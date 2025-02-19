/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2020 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dev2.Common;
using Dev2.Common.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Enums;
using Dev2.Communication;
using Dev2.Runtime.Diagnostics;
using Dev2.Runtime.ESB.Management.Services;
using Dev2.Runtime.Interfaces;
using Dev2.Runtime.ServiceModel.Data;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace Dev2.Tests.Runtime.Services
{
    [TestClass]
    public class DirectDeployTest
    {
        [TestMethod]
        [Owner("Peter Bezuidenhout")]
        [TestCategory("GetResourceID")]
        public void GetResourceID_ShouldReturnEmptyGuid()
        {
            //------------Setup for test--------------------------
            var directDeploy = new DirectDeploy();

            //------------Execute Test---------------------------
            var resId = directDeploy.GetResourceID(new Dictionary<string, StringBuilder>());
            //------------Assert Results-------------------------
            Assert.AreEqual(Guid.Empty, resId);
        }

        [TestMethod]
        [Owner("Peter Bezuidenhout")]
        [TestCategory("GetResourceID")]
        public void GetResourceID_ShouldReturnGuid()
        {
            //------------Setup for test--------------------------
            var serializer = new Dev2JsonSerializer();
            var inputs = new Dictionary<string, StringBuilder>();
            var directDeploy = new DirectDeploy();

            var sourceXml = Dev2.Tests.Runtime.XML.XmlResource.Fetch("WebSource");
            inputs.Add("ResourceDefinition", sourceXml.ToStringBuilder());
            var checkResource = new Resource(sourceXml);
            
            var resourceCatalog = new Mock<IResourceCatalog>();
            resourceCatalog.Setup(catalog => catalog.GetResource(GlobalConstants.ServerWorkspaceID, checkResource.ResourceID)).Returns(checkResource);
            resourceCatalog.Setup(catalog => catalog.GetResourceContents(GlobalConstants.ServerWorkspaceID, checkResource.ResourceID)).Returns(checkResource.ToStringBuilder());
            directDeploy.ResourceCatalog = resourceCatalog.Object;

            //------------Execute Test---------------------------
            var resId = directDeploy.GetResourceID(inputs);
            //------------Assert Results-------------------------
            Assert.AreNotEqual(Guid.Empty, resId);
        }

        [TestMethod]
        [Owner("Peter Bezuidenhout")]
        [TestCategory("GetResourceID")]
        public void GetAuthorizationContextForService_ShouldReturnContext()
        {
            //------------Setup for test--------------------------
            var directDeploy = new DirectDeploy();

            //------------Execute Test---------------------------
            var resId = directDeploy.GetAuthorizationContextForService();
            //------------Assert Results-------------------------
            Assert.AreEqual(AuthorizationContext.DeployTo, resId);
        }

        [TestMethod]
        [Owner("Peter Bezuidenhout")]
        [TestCategory("DirectDeploy_HandlesType")]
        public void DirectDeploy_HandlesType_ExpectName()
        {
            //------------Setup for test--------------------------
            var directDeploy = new DirectDeploy();


            //------------Execute Test---------------------------

            //------------Assert Results-------------------------
            Assert.AreEqual("DirectDeploy", directDeploy.HandlesType());
        }

        [TestMethod]
        [Owner("Peter Bezuidenhout")]
        [TestCategory("DirectDeploy_Execute")]
        public void DirectDeploy_Execute_NullValues_ErrorResult()
        {
            //------------Setup for test--------------------------
            var directDeploy = new DirectDeploy();
            var serializer = new Dev2JsonSerializer();
            //------------Execute Test---------------------------
            var jsonResult = directDeploy.Execute(null, null);
            var result = serializer.Deserialize<IEnumerable<DeployResult>>(jsonResult);
            //------------Assert Results-------------------------
            Assert.IsTrue(result.Any(r => r.HasError));
        }

        [TestMethod]
        [Owner("Peter Bezuidenhout")]
        [TestCategory("DirectDeploy_Execute")]
        public void DirectDeploy_Execute_ResourceIDNotPresent_ErrorResult()
        {
            //------------Setup for test--------------------------
            var values = new Dictionary<string, StringBuilder> { { "resourceIDsToDeploy", null } };
            var directDeploy = new DirectDeploy();
            var serializer = new Dev2JsonSerializer();
            //------------Execute Test---------------------------
            var jsonResult = directDeploy.Execute(values, null);
            var result = serializer.Deserialize<IEnumerable<DeployResult>>(jsonResult);
            //------------Assert Results-------------------------
            Assert.IsTrue(result.Any(r => r.HasError));
        }

        [TestMethod]
        [Owner("Peter Bezuidenhout")]
        [TestCategory("DirectDeploy_Execute")]
        public void Execute_CanNotConnect()
        {
            //------------Setup for test--------------------------
            var serializer = new Dev2JsonSerializer();
            var inputs = new Dictionary<string, StringBuilder>();
            var resourceID = Guid.NewGuid();
            var directDeploy = new DirectDeploy();

            var resourceMock = new Mock<IResource>();
            resourceMock.SetupGet(resource => resource.ResourceID).Returns(resourceID);
            resourceMock.Setup(resource => resource.GetResourcePath(It.IsAny<Guid>())).Returns("Home");

            var testCatalogMock = new Mock<ITestCatalog>();
            var resourceCatalog = new Mock<IResourceCatalog>();
            resourceCatalog.Setup(catalog => catalog.GetResource(GlobalConstants.ServerWorkspaceID, resourceID)).Returns(resourceMock.Object);
            resourceCatalog.Setup(catalog => catalog.GetResourceContents(GlobalConstants.ServerWorkspaceID, resourceID)).Returns(new StringBuilder("Content"));
            testCatalogMock.Setup(a => a.Fetch(It.IsAny<Guid>())).Verifiable();

            var connectionsMock = new Mock<IConnections>();
            connectionsMock.Setup(connection => connection.CanConnectToServer(It.IsAny<Data.ServiceModel.Connection>())).Returns(new ValidationResult { IsValid = false });
            var hubProxy = new Mock<IHubProxy>();
            hubProxy.Setup(proxy => proxy.Invoke("ExecuteCommand", It.IsAny<Envelope>(), It.IsAny<bool>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(new Receipt()));
            var executeMessage = new ExecuteMessage();
            executeMessage.HasError = false;
            executeMessage.Message = new StringBuilder("Great Succesess");
            var execMsg = serializer.Serialize(executeMessage);
            hubProxy.Setup(proxy => proxy.Invoke<string>("FetchExecutePayloadFragment", It.IsAny<FutureReceipt>())).Returns(Task.FromResult(execMsg));
            connectionsMock.Setup(connection => connection.CreateHubProxy(It.IsAny<Data.ServiceModel.Connection>())).Returns(hubProxy.Object);

            directDeploy.Connections = connectionsMock.Object;

            inputs.Add("resourceIDsToDeploy", serializer.SerializeToBuilder(new List<Guid> { resourceID }));
            inputs.Add("destinationEnvironmentId", serializer.SerializeToBuilder(new Data.ServiceModel.Connection { Address = "ABC", UserName = "admin", Password = "password", AuthenticationType = AuthenticationType.Anonymous }));
            inputs.Add("deployTests", serializer.SerializeToBuilder(true));
            directDeploy.TestCatalog = testCatalogMock.Object;
            directDeploy.ResourceCatalog = resourceCatalog.Object;
            //------------Execute Test---------------------------
            var jsonResult = directDeploy.Execute(inputs, null);
            var result = serializer.Deserialize<IEnumerable<DeployResult>>(jsonResult);
            //------------Assert Results-------------------------
            Assert.IsTrue(result.All(r => r.HasError));
        }

        [TestMethod]
        [Owner("Peter Bezuidenhout")]
        [TestCategory("DirectDeploy_Execute")]
        public void Execute_DeployTests_IsNull()
        {
            //------------Setup for test--------------------------
            var serializer = new Dev2JsonSerializer();
            var inputs = new Dictionary<string, StringBuilder>();
            var resourceID = Guid.NewGuid();
            var directDeploy = new DirectDeploy();

            var resourceMock = new Mock<IResource>();
            resourceMock.SetupGet(resource => resource.ResourceID).Returns(resourceID);
            resourceMock.Setup(resource => resource.GetResourcePath(It.IsAny<Guid>())).Returns("Home");

            var testCatalogMock = new Mock<ITestCatalog>();
            var resourceCatalog = new Mock<IResourceCatalog>();
            resourceCatalog.Setup(catalog => catalog.GetResource(GlobalConstants.ServerWorkspaceID, resourceID)).Returns(resourceMock.Object);
            resourceCatalog.Setup(catalog => catalog.GetResourceContents(GlobalConstants.ServerWorkspaceID, resourceID)).Returns(new StringBuilder("Content"));
            testCatalogMock.Setup(a => a.Fetch(It.IsAny<Guid>())).Verifiable();

            var connectionsMock = new Mock<IConnections>();
            connectionsMock.Setup(connection => connection.CanConnectToServer(It.IsAny<Data.ServiceModel.Connection>())).Returns(new ValidationResult { IsValid = true });
            var hubProxy = new Mock<IHubProxy>();
            hubProxy.Setup(proxy => proxy.Invoke("ExecuteCommand", It.IsAny<Envelope>(), It.IsAny<bool>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(new Receipt()));
            var executeMessage = new ExecuteMessage();
            executeMessage.HasError = false;
            executeMessage.Message = new StringBuilder("Great Succesess");
            var execMsg = serializer.Serialize(executeMessage);
            hubProxy.Setup(proxy => proxy.Invoke<string>("FetchExecutePayloadFragment", It.IsAny<FutureReceipt>())).Returns(Task.FromResult(execMsg));
            connectionsMock.Setup(connection => connection.CreateHubProxy(It.IsAny<Data.ServiceModel.Connection>())).Returns(hubProxy.Object);

            directDeploy.Connections = connectionsMock.Object;

            inputs.Add("resourceIDsToDeploy", serializer.SerializeToBuilder(new List<Guid> { resourceID }));
            inputs.Add("destinationEnvironmentId", serializer.SerializeToBuilder(new Data.ServiceModel.Connection { Address = "ABC", UserName = "admin", Password = "password", AuthenticationType = AuthenticationType.Anonymous }));
            directDeploy.TestCatalog = testCatalogMock.Object;
            directDeploy.ResourceCatalog = resourceCatalog.Object;
            //------------Execute Test---------------------------
            var jsonResult = directDeploy.Execute(inputs, null);
            var result = serializer.Deserialize<IEnumerable<DeployResult>>(jsonResult);
            //------------Assert Results-------------------------
            Assert.IsTrue(result.All(r => r.HasError));
        }

        [TestMethod]
        [Owner("Peter Bezuidenhout")]
        [TestCategory("DirectDeploy_Execute")]
        public void Execute_ResourceIDsToDeploy_IsNull()
        {
            //------------Setup for test--------------------------
            var serializer = new Dev2JsonSerializer();
            var inputs = new Dictionary<string, StringBuilder>();
            var resourceID = Guid.NewGuid();
            var directDeploy = new DirectDeploy();

            var resourceMock = new Mock<IResource>();
            resourceMock.SetupGet(resource => resource.ResourceID).Returns(resourceID);
            resourceMock.Setup(resource => resource.GetResourcePath(It.IsAny<Guid>())).Returns("Home");

            var testCatalogMock = new Mock<ITestCatalog>();
            var resourceCatalog = new Mock<IResourceCatalog>();
            resourceCatalog.Setup(catalog => catalog.GetResource(GlobalConstants.ServerWorkspaceID, resourceID)).Returns(resourceMock.Object);
            resourceCatalog.Setup(catalog => catalog.GetResourceContents(GlobalConstants.ServerWorkspaceID, resourceID)).Returns(new StringBuilder("Content"));
            testCatalogMock.Setup(a => a.Fetch(It.IsAny<Guid>())).Verifiable();

            var connectionsMock = new Mock<IConnections>();
            connectionsMock.Setup(connection => connection.CanConnectToServer(It.IsAny<Data.ServiceModel.Connection>())).Returns(new ValidationResult { IsValid = true });
            var hubProxy = new Mock<IHubProxy>();
            hubProxy.Setup(proxy => proxy.Invoke("ExecuteCommand", It.IsAny<Envelope>(), It.IsAny<bool>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(new Receipt()));
            var executeMessage = new ExecuteMessage();
            executeMessage.HasError = false;
            executeMessage.Message = new StringBuilder("Great Succesess");
            var execMsg = serializer.Serialize(executeMessage);
            hubProxy.Setup(proxy => proxy.Invoke<string>("FetchExecutePayloadFragment", It.IsAny<FutureReceipt>())).Returns(Task.FromResult(execMsg));
            connectionsMock.Setup(connection => connection.CreateHubProxy(It.IsAny<Data.ServiceModel.Connection>())).Returns(hubProxy.Object);

            directDeploy.Connections = connectionsMock.Object;

            inputs.Add("resourceIDsToDeploy", serializer.SerializeToBuilder(new List<Guid>()));
            inputs.Add("destinationEnvironmentId", serializer.SerializeToBuilder(new Data.ServiceModel.Connection { Address = "ABC", UserName = "admin", Password = "password", AuthenticationType = AuthenticationType.Anonymous }));
            inputs.Add("deployTests", serializer.SerializeToBuilder(true));
            directDeploy.TestCatalog = testCatalogMock.Object;
            directDeploy.ResourceCatalog = resourceCatalog.Object;
            //------------Execute Test---------------------------
            var jsonResult = directDeploy.Execute(inputs, null);
            var result = serializer.Deserialize<IEnumerable<DeployResult>>(jsonResult);
            //------------Assert Results-------------------------
            Assert.IsTrue(result.All(r => r.HasError));
        }

        [TestMethod]
        [Owner("Peter Bezuidenhout")]
        [TestCategory("DirectDeploy_Execute")]
        public void Execute_ValidValues_ValidResults()
        {
            //------------Setup for test--------------------------
            var serializer = new Dev2JsonSerializer();
            var inputs = new Dictionary<string, StringBuilder>();
            var resourceID = Guid.NewGuid();
            var directDeploy = new DirectDeploy();

            var resourceMock = new Mock<IResource>();
            resourceMock.SetupGet(resource => resource.ResourceID).Returns(resourceID);
            resourceMock.Setup(resource => resource.GetResourcePath(It.IsAny<Guid>())).Returns("Home");

            var testCatalogMock = new Mock<ITestCatalog>();
            var resourceCatalog = new Mock<IResourceCatalog>();
            resourceCatalog.Setup(catalog => catalog.GetResource(GlobalConstants.ServerWorkspaceID, resourceID)).Returns(resourceMock.Object);
            resourceCatalog.Setup(catalog => catalog.GetResourceContents(GlobalConstants.ServerWorkspaceID, resourceID)).Returns(new StringBuilder("Content"));
            testCatalogMock.Setup(a => a.Fetch(It.IsAny<Guid>())).Verifiable();

            var connectionsMock = new Mock<IConnections>();
            connectionsMock.Setup(connection => connection.CanConnectToServer(It.IsAny<Data.ServiceModel.Connection>())).Returns(new ValidationResult { IsValid = true });
            var hubProxy = new Mock<IHubProxy>();
            hubProxy.Setup(proxy => proxy.Invoke("ExecuteCommand", It.IsAny<Envelope>(), It.IsAny<bool>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(new Receipt()));
            var executeMessage = new ExecuteMessage();
            executeMessage.HasError = false;
            executeMessage.Message = new StringBuilder("Great Succesess");
            var execMsg = serializer.Serialize(executeMessage);
            hubProxy.Setup(proxy => proxy.Invoke<string>("FetchExecutePayloadFragment", It.IsAny<FutureReceipt>())).Returns(Task.FromResult(execMsg));
            connectionsMock.Setup(connection => connection.CreateHubProxy(It.IsAny<Data.ServiceModel.Connection>())).Returns(hubProxy.Object);

            directDeploy.Connections = connectionsMock.Object;

            inputs.Add("resourceIDsToDeploy", serializer.SerializeToBuilder(new List<Guid> { resourceID }));
            inputs.Add("destinationEnvironmentId", serializer.SerializeToBuilder(new Data.ServiceModel.Connection { Address = "ABC", UserName = "admin", Password = "password", AuthenticationType = AuthenticationType.Anonymous }));
            inputs.Add("deployTests", serializer.SerializeToBuilder(true));
            inputs.Add("deployTriggers", serializer.SerializeToBuilder(true));
            directDeploy.TestCatalog = testCatalogMock.Object;
            directDeploy.ResourceCatalog = resourceCatalog.Object;
            //------------Execute Test---------------------------
            var jsonResult = directDeploy.Execute(inputs, null);
            var result = serializer.Deserialize<IEnumerable<DeployResult>>(jsonResult);
            //------------Assert Results-------------------------
            Assert.IsTrue(result.All(r => !r.HasError));
        }


    }
}