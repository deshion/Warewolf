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
using System.Collections.Generic;
using System.Linq.Expressions;
using Caliburn.Micro;
using Dev2.Studio.Core.Messages;
using Dev2.Studio.Interfaces;
using Dev2.Studio.Views.ResourceManagement;
using Dev2.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace Dev2.Core.Tests.Utils
{
    [TestClass]
    public class ResourceChangeHandlerTests
    {
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceChangeHandler_Construct")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResourceChangeHandler_Constructor_WithNullEventPublisher_ThrowsArgumentNullException()
        {
            //------------Setup for test--------------------------
            //------------Execute Test---------------------------
            new ResourceChangeHandler(null);
            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceChangeHandler_ShowResourceChanged")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResourceChangeHandlerShowResourceChanged_WithNullResource_ThrowsArgumentNullException()
        {
            //------------Setup for test--------------------------
            var showResourceChangedUtil = CreateShowResourceChangedUtil();
            //------------Execute Test---------------------------
            showResourceChangedUtil.ShowResourceChanged(null, new List<string>());
            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceChangeHandler_ShowResourceChanged")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResourceChangeHandler_ShowResourceChanged_WithNullList_ThrowsArgumentNullException()
        {
            //------------Setup for test--------------------------
            var showResourceChangedUtil = CreateShowResourceChangedUtil();
            //------------Execute Test---------------------------
            showResourceChangedUtil.ShowResourceChanged(new Mock<IContextualResourceModel>().Object, null);
            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceChangeHandler_ShowResourceChanged")]
        public void ResourceChangeHandler_ShowResourceChanged_WithOneDependant_FiresAddWorkSurfaceMessageMessage()
        {
            //------------Setup for test--------------------------
            var mockAggregator = new Mock<IEventAggregator>();
            var showResourceChangedUtil = CreateShowResourceChangedUtil(mockAggregator.Object);
            var mockResource = new Mock<IContextualResourceModel>();
            var mockEnvironmentModel = new Mock<IServer>();
            var mockResourceRepository = new Mock<IResourceRepository>();
            var mockResourceModelDependant = new Mock<IResourceModel>();
            var server = new Mock<IServer>();
            server.Setup(a => a.DisplayName).Returns("LocalHost");
            var shell = new Mock<IShellViewModel>();
            CustomContainer.Register<IShellViewModel>(shell.Object);
            shell.Setup(a => a.LocalhostServer).Returns(server.Object);
            shell.Setup(a => a.ActiveServer).Returns(server.Object);
            shell.Setup(a => a.OpenResourceAsync(It.IsAny<Guid>(), server.Object)).Verifiable();
            mockResourceModelDependant.Setup(model => model.ResourceName).Returns("MyResource");
            mockResourceRepository.Setup(r => r.FindSingle(It.IsAny<Expression<Func<IResourceModel, bool>>>(), false, false)).Returns(mockResourceModelDependant.Object);
            mockEnvironmentModel.Setup(model => model.ResourceRepository).Returns(mockResourceRepository.Object);
            mockResource.Setup(model => model.Environment).Returns(mockEnvironmentModel.Object);
            //------------Execute Test---------------------------
            var mock = new Mock<IResourceChangedDialog>();
            mock.Setup(dialog => dialog.OpenDependencyGraph).Returns(true);
            showResourceChangedUtil.ShowResourceChanged(mockResource.Object, new List<string> { mockResourceModelDependant.Object.ID.ToString() }, mock.Object);
            //------------Assert Results-------------------------
            shell.Verify(model => model.OpenResourceAsync(It.IsAny<Guid>(), server.Object));
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ResourceChangeHandler_ShowResourceChanged")]
        public void ResourceChangeHandler_ShowResourceChanged_WithMoreThanOneDependant_FiresShowReverseDependencyVisualizerMessage()
        {
            //------------Setup for test--------------------------
            var mockAggregator = new Mock<IEventAggregator>();
            var showResourceChangedUtil = CreateShowResourceChangedUtil(mockAggregator.Object);
            var mockResource = new Mock<IContextualResourceModel>();
            var mockEnvironmentModel = new Mock<IServer>();
            var mockResourceRepository = new Mock<IResourceRepository>();
            var mockResourceModelDependant = new Mock<IResourceModel>();
            mockAggregator.Setup(a => a.Publish(It.IsAny<ShowReverseDependencyVisualizer>())).Verifiable();
            mockResourceModelDependant.Setup(model => model.ResourceName).Returns("MyResource");
            mockResourceRepository.Setup(r => r.FindSingle(It.IsAny<Expression<Func<IResourceModel, bool>>>(), false, false)).Returns(mockResourceModelDependant.Object);
            mockEnvironmentModel.Setup(model => model.ResourceRepository).Returns(mockResourceRepository.Object);
            mockResource.Setup(model => model.Environment).Returns(mockEnvironmentModel.Object);
            //------------Execute Test---------------------------
            var mock = new Mock<IResourceChangedDialog>();
            mock.Setup(dialog => dialog.OpenDependencyGraph).Returns(true);
            showResourceChangedUtil.ShowResourceChanged(mockResource.Object, new List<string> { "MyResource", "MyOtherResource" }, mock.Object);
            //------------Assert Results-------------------------
            mockAggregator.Verify(aggregator => aggregator.Publish(It.IsAny<ShowReverseDependencyVisualizer>()), Times.Once());
        }

        static ResourceChangeHandler CreateShowResourceChangedUtil()
        {
            return CreateShowResourceChangedUtil(new Mock<IEventAggregator>().Object);
        }

        static ResourceChangeHandler CreateShowResourceChangedUtil(IEventAggregator eventAggregator)
        {
            return new ResourceChangeHandler(eventAggregator);
        }
    }
}
