﻿#pragma warning disable
/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Dev2.Common;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Interfaces.Infrastructure;
using Dev2.Communication;
using Dev2.DynamicServices;
using Dev2.Runtime.Hosting;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Workspaces;
using System;
using System.Collections.Generic;
using System.Text;
using Dev2.Common.Interfaces.Enums;
using Dev2.Runtime.Interfaces;


namespace Dev2.Runtime.ESB.Management.Services
{

    public class SaveExchangeServiceSource : IEsbManagementEndpoint
    {
        IExplorerServerResourceRepository _serverExplorerRepository;
        IResourceCatalog _resourceCatalogue;

        public Guid GetResourceID(Dictionary<string, StringBuilder> requestArgs) => Guid.Empty;

        public AuthorizationContext GetAuthorizationContextForService() => AuthorizationContext.Contribute;

        public StringBuilder Execute(Dictionary<string, StringBuilder> values, IWorkspace theWorkspace)
        {
            var msg = new ExecuteMessage();
            var serializer = new Dev2JsonSerializer();
            try
            {

                Dev2Logger.Info("Save Exchange Service Source", GlobalConstants.WarewolfInfo);

                values.TryGetValue("ExchangeSource", out StringBuilder resourceDefinition);

                var src = serializer.Deserialize<ExchangeSourceDefinition>(resourceDefinition);
                var con = new ExchangeSource
                {
                    AutoDiscoverUrl = src.AutoDiscoverUrl,
                    UserName = src.UserName,
                    Password = src.Password,
                    Timeout = src.Timeout,
                    ResourceName = src.ResourceName,
                    ResourceID = src.Id,
                    Type = enSourceType.ExchangeSource,
                    ResourceType = "ExchangeSource"
                };
                ResourceCatalog.Instance.SaveResource(GlobalConstants.ServerWorkspaceID, con, src.Path);
                ServerExplorerRepo.UpdateItem(con);

                msg.HasError = false;
            }
            catch (Exception err)
            {
                msg.HasError = true;
                msg.Message = new StringBuilder(err.Message);
                Dev2Logger.Error(err, GlobalConstants.WarewolfError);
            }

            return serializer.SerializeToBuilder(msg);
        }

        public IExplorerServerResourceRepository ServerExplorerRepo
        {
            get => _serverExplorerRepository ?? ServerExplorerRepository.Instance;
            set => _serverExplorerRepository = value;
        }

        public IResourceCatalog ResourceCatalogue
        {
            get => _resourceCatalogue ?? ResourceCatalog.Instance;
            set => _resourceCatalogue = value;
        }

        public DynamicService CreateServiceEntry() => EsbManagementServiceEntry.CreateESBManagementServiceEntry(HandlesType(), "<DataList><Roles ColumnIODirection=\"Input\"/><EmailServiceSource ColumnIODirection=\"Input\"/><WorkspaceID ColumnIODirection=\"Input\"/><Dev2System.ManagmentServicePayload ColumnIODirection=\"Both\"></Dev2System.ManagmentServicePayload></DataList>");

        public string HandlesType() => "SaveExchangeServiceSource";
    }
}