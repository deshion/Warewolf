/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2021 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/


#pragma warning disable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Transactions;
using System.Xml.Linq;
using ChinhDo.Transactions;
using Dev2.Common;
using Dev2.Common.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Hosting;
using Dev2.Common.Interfaces.Infrastructure.Providers.Errors;
using Dev2.Common.Interfaces.Infrastructure.SharedModels;
using Dev2.Common.Interfaces.Versioning;
using Dev2.Common.Wrappers;
using Dev2.Data.ServiceModel.Messages;
using Dev2.DynamicServices;
using Dev2.DynamicServices.Objects;
using Dev2.Runtime.Compiler;
using Dev2.Runtime.Hosting;
using Dev2.Runtime.Interfaces;
using Dev2.Runtime.Security;
using Dev2.Runtime.ServiceModel.Data;
using Warewolf.Resource.Errors;
using static Dev2.Common.Interfaces.WarewolfExecutionEnvironmentException;

namespace Dev2.Runtime.ResourceCatalogImpl
{
    class ResourceSaveProvider : IResourceSaveProvider
    {
        readonly IResourceCatalog _resourceCatalog;
        readonly IServerVersionRepository _serverVersionRepository;
        readonly FileWrapper _dev2FileWrapper = new FileWrapper();
        public ResourceSaveProvider(IResourceCatalog resourceCatalog, IServerVersionRepository serverVersionRepository)
        {
            _resourceCatalog = resourceCatalog;
            _serverVersionRepository = serverVersionRepository;
        }

        public ResourceCatalogResult SaveResource(Guid workspaceID, StringBuilder resourceXml, string savedPath) => SaveResource(workspaceID, resourceXml, savedPath, "", "");

        public ResourceCatalogResult SaveResource(Guid workspaceID, StringBuilder resourceXml, string savedPath, string reason) => SaveResource(workspaceID, resourceXml, savedPath, reason, "");
        public ResourceCatalogResult SaveResource(Guid workspaceID, StringBuilder resourceXml, string savedPath, string reason, string user)
        {
            try
            {
                if (resourceXml == null || resourceXml.Length == 0)
                {
                    throw new ArgumentNullException(nameof(resourceXml));
                }

                var @lock = Common.GetWorkspaceLock(workspaceID);
                lock (@lock)
                {
                    var xml = resourceXml.ToXElement();

                    var resource = new Resource(xml);
                    GlobalConstants.InvalidateCache(resource.ResourceID);
                    Dev2Logger.Info("Save Resource." + resource, GlobalConstants.WarewolfInfo);
                    _serverVersionRepository.StoreVersion(resource, user, reason, workspaceID, savedPath);

                    resource.UpgradeXml(xml, resource);

                    var result = xml.ToStringBuilder();

                    return CompileAndSave(workspaceID, resource, result, savedPath, reason);
                }
            }
            catch (Exception err)
            {
                Dev2Logger.Error("Save Error", err, GlobalConstants.WarewolfError);
                throw;
            }
        }

        public ResourceCatalogResult SaveResource(Guid workspaceID, IResource resource, string savedPath) => SaveResource(workspaceID, resource, savedPath, "", "");

        public ResourceCatalogResult SaveResource(Guid workspaceID, IResource resource, string savedPath, string reason, string user)
        {
            _serverVersionRepository.StoreVersion(resource, user, reason, workspaceID, savedPath);

            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            var @lock = Common.GetWorkspaceLock(workspaceID);
            lock (@lock)
            {
                if (resource.ResourceID == Guid.Empty)
                {
                    resource.ResourceID = Guid.NewGuid();
                }
                GlobalConstants.InvalidateCache(resource.ResourceID);
                savedPath = Common.SanitizePath(savedPath);
                var result = resource.ToStringBuilder();
                return CompileAndSave(workspaceID, resource, result, savedPath, reason);
            }
        }

        public Action<IResource> ResourceSaved { get; set; }
        public Action<Guid, IList<ICompileMessageTO>> SendResourceMessages { get; set; }

        public ResourceCatalogResult SaveResources(Guid serverWorkspaceID, List<DuplicateResourceTO> resourceMap, bool overrideExisting)
        {
            return SaveResources(serverWorkspaceID, resourceMap, overrideExisting, string.Empty, string.Empty);
        }

        public ResourceCatalogResult SaveResources(Guid workspaceID, List<DuplicateResourceTO> resourceMap, bool overrideExisting , string reason, string user)
        {
            ResourceCatalogResult saveResult = null;
            Dev2.Common.Utilities.PerformActionInsideImpersonatedContext(Dev2.Common.Utilities.ServerUser, () => { PerformSaveBulkResult(out saveResult, workspaceID, resourceMap, overrideExisting, user, reason); });
            return saveResult;
        }

        public ResourceCatalogResult SaveResource(Guid workspaceID, IResource resource, StringBuilder contents, string savedPath) => SaveResource(workspaceID, resource, contents, savedPath, "", "");


        public ResourceCatalogResult SaveResource(Guid workspaceID, IResource resource, StringBuilder contents, string savedPath, string reason, string user)
        { 
            _serverVersionRepository.StoreVersion(resource, user, reason, workspaceID, savedPath);
            ResourceCatalogResult saveResult = null;
            Dev2.Common.Utilities.PerformActionInsideImpersonatedContext(Dev2.Common.Utilities.ServerUser, () => { PerformSaveResult(out saveResult, workspaceID, resource, contents, true, savedPath); });
            return saveResult;
        }

        public ResourceCatalogResult SaveImpl(Guid workspaceID, IResource resource, StringBuilder contents, bool overwriteExisting) => SaveImpl(workspaceID, resource, contents, overwriteExisting, "", "");
        public ResourceCatalogResult SaveImpl(Guid workspaceID, IResource resource, StringBuilder contents, bool overwriteExisting, string savedPath) => SaveImpl(workspaceID, resource, contents, overwriteExisting, savedPath, "");
        public ResourceCatalogResult SaveImpl(Guid workspaceID, IResource resource, StringBuilder contents, bool overwriteExisting, string savedPath, string reason)
        {
            ResourceCatalogResult saveResult = null;
            Dev2.Common.Utilities.PerformActionInsideImpersonatedContext(Dev2.Common.Utilities.ServerUser, () => { PerformSaveResult(out saveResult, workspaceID, resource, contents, overwriteExisting, savedPath, reason); });
            return saveResult;
        }

        ResourceCatalogResult CompileAndSave(Guid workspaceID, IResource resource, StringBuilder contents, string savedPath = "", string reason = "")
        {
            // Find the service before edits ;)
            var beforeService = _resourceCatalog.GetDynamicObjects<DynamicService>(workspaceID, resource.ResourceID).FirstOrDefault();

            ServiceAction beforeAction = null;
            if (beforeService != null)
            {
                beforeAction = beforeService.Actions.FirstOrDefault();
                if (reason?.Equals(GlobalConstants.SaveReasonForDeploy) ?? false)
                {
                    beforeService.DisplayName = resource.ResourceName;
                    beforeService.Name = resource.ResourceName;
                }
            }

            var result = _resourceCatalog.SaveImpl(workspaceID, resource, contents, savedPath, reason);

            if (result != null && result.Status == ExecStatus.Success)
            {
                if (workspaceID == GlobalConstants.ServerWorkspaceID)
                {
                    CompileTheResourceAfterSave(workspaceID, resource, contents, beforeAction);
                    UpdateResourceDependencies(resource, contents);
                    SavedResourceCompileMessage(workspaceID, resource, result.Message);
                }
                if (ResourceSaved != null && workspaceID == GlobalConstants.ServerWorkspaceID)
                {
                    ResourceSaved(resource);
                }

            }

            return result;
        }

        void UpdateResourceDependencies(IResource resource, StringBuilder contents)
        {
            resource.LoadDependencies(contents.ToXElement());
        }

        protected void CompileTheResourceAfterSave(Guid workspaceID, IResource resource, StringBuilder contents, ServiceAction beforeAction)
        {
            if (beforeAction != null)
            {
                // Compile the service 
                var smc = new ServiceModelCompiler();

                var messages = GetCompileMessages(resource, contents, beforeAction, smc);
                if (messages != null)
                {
                    var keys = _resourceCatalog.WorkspaceResources.Keys.ToList();
                    CompileMessageRepo.Instance.AddMessage(workspaceID, messages); //Sends the message for the resource being saved

                    var dependsMessageList = new List<ICompileMessageTO>();
                    keys.ForEach(workspace =>
                    {
                        dependsMessageList.AddRange(UpdateDependantResourceWithCompileMessages(workspace, resource, messages));
                    });
                    _resourceCatalog.SendResourceMessages?.Invoke(resource.ResourceID, dependsMessageList);
                }
            }
        }
        IEnumerable<ICompileMessageTO> UpdateDependantResourceWithCompileMessages(Guid workspaceID, IResource resource, IList<ICompileMessageTO> messages)
        {
            var resourceId = resource.ResourceID;
            var dependants = _resourceCatalog.GetDependentsAsResourceForTrees(workspaceID, resourceId);
            var dependsMessageList = new List<ICompileMessageTO>();
            foreach (var dependant in dependants)
            {
                var affectedResource = _resourceCatalog.GetResource(workspaceID, dependant.ResourceID);
                foreach (var compileMessageTO in messages)
                {
                    compileMessageTO.WorkspaceID = workspaceID;
                    compileMessageTO.UniqueID = dependant.UniqueID;
                    if (affectedResource != null)
                    {
                        compileMessageTO.ServiceName = affectedResource.ResourceName;
                        compileMessageTO.ServiceID = affectedResource.ResourceID;
                    }
                    dependsMessageList.Add(compileMessageTO.Clone());
                }
                if (affectedResource != null)
                {
                    Common.UpdateResourceXml(_resourceCatalog, workspaceID, affectedResource, messages);
                }
            }
            CompileMessageRepo.Instance.AddMessage(workspaceID, dependsMessageList);
            return dependsMessageList;
        }


        static IList<ICompileMessageTO> GetCompileMessages(IResource resource, StringBuilder contents, ServiceAction beforeAction, ServiceModelCompiler smc)
        {
            var messages = new List<ICompileMessageTO>();
            switch (beforeAction.ActionType)
            {
                case enActionType.Workflow:
                    messages.AddRange(smc.Compile(resource.ResourceID, ServerCompileMessageType.WorkflowMappingChangeRule, beforeAction.ResourceDefinition, contents));
                    break;
                case enActionType.BizRule:
                    break;
                case enActionType.InvokeStoredProc:
                    break;
                case enActionType.InvokeWebService:
                    break;
                case enActionType.InvokeDynamicService:
                    break;
                case enActionType.InvokeManagementDynamicService:
                    break;
                case enActionType.InvokeServiceMethod:
                    break;
                case enActionType.Plugin:
                    break;
                case enActionType.ComPlugin:
                    break;
                case enActionType.Switch:
                    break;
                case enActionType.Unknown:
                    break;
                case enActionType.RemoteService:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return messages;
        }

        void SavedResourceCompileMessage(Guid workspaceID, IResource resource, string saveMessage)
        {
            var savedResourceCompileMessage = new List<ICompileMessageTO>
            {
                new CompileMessageTO
                {
                    ErrorType = ErrorType.None,
                    MessageID = Guid.NewGuid(),
                    MessagePayload = saveMessage,
                    ServiceID = resource.ResourceID,
                    ServiceName = resource.ResourceName,
                    MessageType = CompileMessageType.ResourceSaved,
                    WorkspaceID = workspaceID,
                }
            };

            CompileMessageRepo.Instance.AddMessage(workspaceID, savedResourceCompileMessage);
        }
        XElement SaveToDisk(IResource resource, StringBuilder contents, string directoryName, TxFileManager fileManager)
        {
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            if (_dev2FileWrapper.Exists(resource.FilePath))
            {
                // Remove readonly attribute if it is set
                var attributes = _dev2FileWrapper.GetAttributes(resource.FilePath);
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    _dev2FileWrapper.SetAttributes(resource.FilePath, attributes ^ FileAttributes.ReadOnly);
                }
            }

            var xml = contents.ToXElement();
            xml = resource.UpgradeXml(xml, resource);
            var result = xml.ToStringBuilder();

            var signedXml = HostSecurityProvider.Instance.SignXml(result);

            lock (Common.GetFileLock(resource.FilePath))
            {
                signedXml.WriteToFile(resource.FilePath, Encoding.UTF8, fileManager);
            }
            return xml;
        }
        static bool AddToCatalog(IResource resource, List<IResource> resources, TxFileManager fileManager, XElement xml)
        {
            var index = resources.IndexOf(resource);
            var updated = false;
            if (index != -1)
            {
                var existing = resources[index];
                if (!string.Equals(existing.FilePath, resource.FilePath, StringComparison.CurrentCultureIgnoreCase))
                {
                    fileManager.Delete(existing.FilePath);
                }
                resources.RemoveAt(index);
                updated = true;
            }
            resource.GetInputsOutputs(xml);
            resource.ReadDataList(xml);
            resource.SetIsNew(xml);
            resource.UpdateErrorsBasedOnXML(xml);

            resources.Add(resource);
            return updated;
        }

        void PerformSaveBulkResult(out ResourceCatalogResult saveResult, Guid workspaceID, List<DuplicateResourceTO> resourceMap, bool overwriteExisting, string user = "", string reason = "")
        {
            var resourceResults = new List<ResourceCatalogResult>();
            foreach (var mapper in resourceMap)
            {
                var resource = mapper.NewResource;
                var destination = mapper.DestinationPath;
                var content = mapper.ResourceContents;

                _serverVersionRepository.StoreVersion(resource, user, reason, workspaceID, destination);
                PerformSaveResult(out saveResult, workspaceID, resource, content, overwriteExisting, destination, reason);
                ServerExplorerRepository.Instance.UpdateItem(resource);
                resourceResults.Add(saveResult);
            }
            //TODO: all these results should be returned with this method
            saveResult = resourceResults.Last();
        }

        void PerformSaveResult(out ResourceCatalogResult saveResult, Guid workspaceID, IResource resource, StringBuilder contents, bool overwriteExisting, string savedPath, string reason = "")
        {
            var fileManager = new TxFileManager();
            using (TransactionScope tx = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    var resources = _resourceCatalog.GetResources(workspaceID);
                    var conflicting = resources.FirstOrDefault(r => resource.ResourceID != r.ResourceID && r.GetResourcePath(workspaceID) != null && r.GetResourcePath(workspaceID).Equals(savedPath + "\\" + resource.ResourceName, StringComparison.InvariantCultureIgnoreCase) && r.ResourceName.Equals(resource.ResourceName, StringComparison.InvariantCultureIgnoreCase));
                    if (conflicting != null && !conflicting.IsNewResource || conflicting != null && !overwriteExisting)
                    {
                        saveResult = ResourceCatalogResultBuilder.CreateDuplicateMatchResult(string.Format(ErrorResource.TypeConflict, conflicting.ResourceType));
                        return;
                    }
                    contents = GetExistingResource(resource, contents, reason, resources);

                    var directoryName = SetResourceFilePath(workspaceID, resource, ref savedPath);


                    var xml = SaveToDisk(resource, contents, directoryName, fileManager);


                    var updated = AddToCatalog(resource, resources, fileManager, xml);

                    _resourceCatalog.AddToActivityCache(resource);

                    Dev2Logger.Debug($"Removing Execution Plan for {resource.ResourceID} for workspace {workspaceID}", GlobalConstants.WarewolfDebug);
                    _resourceCatalog.RemoveFromResourceActivityCache(workspaceID, resource);
                    Dev2Logger.Debug($"Removed Execution Plan for {resource.ResourceID} for workspace {workspaceID}", GlobalConstants.WarewolfDebug);
                    Dev2Logger.Debug($"Adding Execution Plan for {resource.ResourceID} for workspace {workspaceID}", GlobalConstants.WarewolfDebug);
                    _resourceCatalog.Parse(workspaceID, resource);
                    Dev2Logger.Debug($"Added Execution Plan for {resource.ResourceID} for workspace {workspaceID}", GlobalConstants.WarewolfDebug);
                    tx.Complete();
                    saveResult = ResourceCatalogResultBuilder.CreateSuccessResult($"{(updated ? "Updated" : "Added")} {resource.ResourceType} '{resource.ResourceName}'");
                }
                catch (Exception e)
                {
                    Dev2Logger.Warn($"Error saving {resource.ResourceName}. " + e.Message, GlobalConstants.WarewolfWarn);
                    Transaction.Current.Rollback();
                    //the process should not be terminated because of just one failure 
                    saveResult = ResourceCatalogResultBuilder.CreateFailResult(string.Format(ErrorResource.ErrorDuringSaveCallback, resource.ResourceID) +"' message "+e.Message);
                }
            }
        }

        static StringBuilder GetExistingResource(IResource resource, StringBuilder contents, string reason, List<IResource> resources)
        {
            var res = resources.FirstOrDefault(p => p.ResourceID == resource.ResourceID);
            var Outputcontents = contents;
            if (res != null && res.ResourceName != resource.ResourceName)//Found Existing resource
            {
                var resourceXml = contents.ToXElement();
                if (!reason?.Equals(GlobalConstants.SaveReasonForDeploy) ?? true)
                {
                    resourceXml.SetAttributeValue("Name", res.ResourceName);
                    resourceXml.SetElementValue("DisplayName", res.ResourceName);
                    var actionElement = resourceXml.Element("Action");
                    var xamlElement = actionElement?.Element("XamlDefinition");
                    if (xamlElement != null)
                    {
                        var xamlContent = xamlElement.Value;
                        xamlElement.Value = xamlContent.
                            Replace("x:Class=\"" + resource.ResourceName + "\"", "x:Class=\"" + res.ResourceName + "\"")
                            .Replace("Flowchart DisplayName=\"" + resource.ResourceName + "\"", "Flowchart DisplayName=\"" + res.ResourceName + "\"");
                    }
                    resource.ResourceName = res.ResourceName;
                    Outputcontents = resourceXml.ToStringBuilder();
                }
            }

            return Outputcontents;
        }

        public string SetResourceFilePath(Guid workspaceID, IResource resource, ref string savedPath)
        {
            if (savedPath.EndsWith("\\"))
            {
                savedPath = savedPath.TrimEnd('\\');
            }
            if (savedPath.StartsWith("\\"))
            {
                savedPath = savedPath.TrimStart('\\');
            }
            var workspacePath = EnvironmentVariables.GetWorkspacePath(workspaceID);
            var directoryName = Path.Combine(workspacePath, savedPath);
            var resourceFilePath = Path.Combine(directoryName, resource.ResourceName + ".bite");
            resource.FilePath = resourceFilePath;
            return directoryName;
        }

    }
}