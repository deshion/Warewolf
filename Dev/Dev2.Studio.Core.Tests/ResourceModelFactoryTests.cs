/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using Dev2.Common.Interfaces.Security;
using Dev2.Studio.Core.Factories;
using Dev2.Studio.Interfaces;
using Dev2.Studio.Interfaces.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace Dev2.Core.Tests
{
    [TestClass]
    public class ResourceModelFactoryTests
    {
        [TestMethod]
        [Owner("Trevor Williams-Ros")]
        [TestCategory("ResourceModelFactory_CreateResourceModel")]
        public void ResourceModelFactory_CreateResourceModel_UserPermissions_Contribute()
        {
            Verify_CreateResourceModel_UserPermissions(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel));
            Verify_CreateResourceModel_UserPermissions(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "WorkflowService", "iconPath", "displayName"));
            Verify_CreateResourceModel_UserPermissions(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "WorkflowService", "displayName"));
            Verify_CreateResourceModel_UserPermissions(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "WorkflowService", "resourceName", "displayName"));
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceModelFactory_CreateResourceModel")]
        public void ResourceModelFactory_CreateResourceModel_WorkflowService()
        {
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "WorkflowService", "displayName"), ResourceType.WorkflowService, "displayName", "", "WorkflowService");
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "WorkflowService", "resourceName", "displayName"), ResourceType.WorkflowService, "displayName", "resourceName", "WorkflowService");
        }

        [TestMethod]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("ResourceModelFactory_CreateResourceModel")]
        public void ResourceModelFactory_CreateResourceModel_OauthSource()
        {
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "DropboxSource", "SelectedOauthSource"), ResourceType.Source, "SelectedOauthSource", "", "DropboxSource");
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceModelFactory_CreateResourceModel")]
        public void ResourceModelFactory_CreateResourceModel_ResourceService()
        {
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "ResourceService", "displayName"), ResourceType.Service, "PluginService", "", "PluginService");
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "ResourceService", "resourceName", "displayName"), ResourceType.Service, "PluginService", "resourceName", "PluginService");
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceModelFactory_CreateResourceModel")]
        public void ResourceModelFactory_CreateResourceModel_WebService()
        {
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "WebService", "displayName"), ResourceType.Service, "displayName", "", "WebService");
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "WebService", "resourceName", "displayName"), ResourceType.Service, "displayName", "resourceName", "WebService");
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceModelFactory_CreateResourceModel")]
        public void ResourceModelFactory_CreateResourceModel_Server()
        {
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "Server", "displayName"), ResourceType.Server, "displayName", "", "ServerSource");
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "Server", "resourceName", "displayName"), ResourceType.Server, "displayName", "resourceName", "ServerSource");
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceModelFactory_CreateResourceModel")]
        public void ResourceModelFactory_CreateResourceModel_DbSource()
        {
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "DbSource", "displayName"), ResourceType.Source, "displayName", "", "DbSource");
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "DbSource", "resourceName", "displayName"), ResourceType.Source, "displayName", "resourceName", "DbSource");
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceModelFactory_CreateResourceModel")]
        public void ResourceModelFactory_CreateResourceModel_ResourceSource()
        {
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "ResourceSource", "displayName"), ResourceType.Source, "Plugin", "", "PluginSource");
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "ResourceSource", "resourceName", "displayName"), ResourceType.Source, "Plugin", "resourceName", "PluginSource");
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceModelFactory_CreateResourceModel")]
        public void ResourceModelFactory_CreateResourceModel_WebSource()
        {
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "WebSource", "displayName"), ResourceType.Source, "displayName", "", "WebSource");
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "WebSource", "resourceName", "displayName"), ResourceType.Source, "displayName", "resourceName", "WebSource");
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceModelFactory_CreateResourceModel")]
        public void ResourceModelFactory_CreateResourceModel_EmailSource()
        {
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "EmailSource", "displayName"), ResourceType.Source, "displayName", "", "EmailSource");
            Verify_CreateResourceModel_ResourceType(environmentModel => ResourceModelFactory.CreateResourceModel(environmentModel, "EmailSource", "resourceName", "displayName"), ResourceType.Source, "displayName", "resourceName", "EmailSource");
        }

        static void Verify_CreateResourceModel_UserPermissions(Func<IServer, IContextualResourceModel> createResourceModel)
        {
            //------------Setup for test--------------------------
            var environmentModel = new Mock<IServer>();

            //------------Execute Test---------------------------
            var resourceModel = createResourceModel?.Invoke(environmentModel.Object);

            //------------Assert Results-------------------------
            Assert.AreEqual(Permissions.Contribute, resourceModel.UserPermissions);
        }

        static void Verify_CreateResourceModel_ResourceType(Func<IServer, IContextualResourceModel> createResourceModel, ResourceType resourceType, string displayName, string resourceName, string serverResourceName)
        {
            //------------Setup for test--------------------------
            var environmentModel = new Mock<IServer>();

            //------------Execute Test---------------------------
            var resourceModel = createResourceModel?.Invoke(environmentModel.Object);

            //------------Assert Results-------------------------
            Assert.AreEqual(resourceType, resourceModel.ResourceType);
            Assert.AreEqual(displayName, resourceModel.DisplayName);
            Assert.AreEqual(resourceName, resourceModel.ResourceName);
            Assert.AreEqual(serverResourceName, resourceModel.ServerResourceType);
        }
    }
}
