﻿/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2021 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/


using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Communication;
using Dev2.Controller;
using Dev2.Runtime.Hosting;
using Dev2.Studio.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Warewolf.Studio.Core;



namespace Warewolf.Studio.ViewModels.Tests
{
    [TestClass]
    public class RequestServiceNameViewModelTests
    {
        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RequestServiceNameViewModel_CreateAsync_NullParameters_ShouldError()
        {
            //------------Setup for test--------------------------


            //------------Execute Test---------------------------
            RequestServiceNameViewModel.CreateAsync(null, "", "");
            //------------Assert Results-------------------------
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_CreateAsync_ParametersPassed_ShouldConstructCorrectly()
        {
            //------------Setup for test--------------------------
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            //------------Execute Test---------------------------
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(new Mock<IEnvironmentViewModel>().Object, "", "");
            //------------Assert Results-------------------------
            Assert.IsNotNull(requestServiceNameViewModel);
            Assert.IsNotNull(requestServiceNameViewModel.OkCommand);
            Assert.IsNotNull(requestServiceNameViewModel.CancelCommand);
            Assert.AreEqual("", requestServiceNameViewModel.Name);
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_ShowSaveDialog_NoItemSelected_ShouldReturnResourceNameNoPath()
        {
            //------------Setup for test--------------------------
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            mockEnvironmentModel.Setup(model => model.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            mockEnvironmentModel.Setup(model => model.LoadDialogAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();
            requestServiceNameViewModel.Name = "TestResource";
            //------------Execute Test---------------------------
            requestServiceNameViewModel.OkCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsNotNull(requestServiceNameViewModel.ResourceName);
            Assert.AreEqual("", requestServiceNameViewModel.ResourceName.Path);
            Assert.AreEqual("TestResource", requestServiceNameViewModel.ResourceName.Name);
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_ShowSaveDialog_ItemSelected_ShouldReturnResourceNameWithPath()
        {
            //------------Setup for test--------------------------
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            mockEnvironmentModel.Setup(model => model.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();
            requestServiceNameViewModel.Name = "TestResource";
            var mockExplorerTreeItem = new Mock<IExplorerTreeItem>();
            mockExplorerTreeItem.Setup(item => item.ResourceType).Returns("Folder");
            mockExplorerTreeItem.Setup(item => item.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            mockExplorerTreeItem.Setup(item => item.ResourceName).Returns("MyFolder");
            requestServiceNameViewModel.SingleEnvironmentExplorerViewModel.SelectedItem = mockExplorerTreeItem.Object;
            //------------Execute Test---------------------------
            requestServiceNameViewModel.OkCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsNotNull(requestServiceNameViewModel.ResourceName);
            Assert.AreEqual("MyFolder\\", requestServiceNameViewModel.ResourceName.Path);
            Assert.AreEqual("TestResource", requestServiceNameViewModel.ResourceName.Name);
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_ShowSaveDialog_ItemSelectedHasParent_ShouldReturnResourceNameWithPathWithParent()
        {
            //------------Setup for test--------------------------
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            mockEnvironmentModel.Setup(model => model.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();
            requestServiceNameViewModel.Name = "TestResource";
            var mockExplorerParentTreeItem = new Mock<IExplorerTreeItem>();
            mockExplorerParentTreeItem.Setup(item => item.ResourceType).Returns("Folder");
            mockExplorerParentTreeItem.Setup(item => item.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            mockExplorerParentTreeItem.Setup(item => item.ResourceName).Returns("MyParentFolder");

            var mockExplorerTreeItem = new Mock<IExplorerTreeItem>();
            mockExplorerTreeItem.Setup(item => item.ResourceType).Returns("Folder");
            mockExplorerTreeItem.Setup(item => item.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            mockExplorerTreeItem.Setup(item => item.ResourceName).Returns("MyFolder");
            mockExplorerTreeItem.Setup(item => item.Parent).Returns(mockExplorerParentTreeItem.Object);

            requestServiceNameViewModel.SingleEnvironmentExplorerViewModel.SelectedItem = mockExplorerTreeItem.Object;
            //------------Execute Test---------------------------
            requestServiceNameViewModel.OkCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsNotNull(requestServiceNameViewModel.ResourceName);
            Assert.AreEqual("MyParentFolder\\MyFolder\\", requestServiceNameViewModel.ResourceName.Path);
            Assert.AreEqual("TestResource", requestServiceNameViewModel.ResourceName.Name);
            Assert.AreEqual("Can only save to folders or root", requestServiceNameViewModel.ErrorMessage);
        }

        [TestMethod, Timeout(60000)]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_ShowSaveDialog_ItemSelectedHasMultipleParents_ShouldReturnResourceNameWithPathWithParent()
        {
            //------------Setup for test--------------------------
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            mockEnvironmentModel.Setup(model => model.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();

            requestServiceNameViewModel.Name = "TestResource";
            var mockExplorerTreeItemA = new Mock<IExplorerTreeItem>();
            mockExplorerTreeItemA.Setup(item => item.ResourceType).Returns("Folder");
            mockExplorerTreeItemA.Setup(item => item.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            mockExplorerTreeItemA.Setup(item => item.ResourceName).Returns("A");

            var mockExplorerTreeItemB = new Mock<IExplorerTreeItem>();
            mockExplorerTreeItemB.Setup(item => item.ResourceType).Returns("Folder");
            mockExplorerTreeItemB.Setup(item => item.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            mockExplorerTreeItemB.Setup(item => item.ResourceName).Returns("B");
            mockExplorerTreeItemB.Setup(item => item.Parent).Returns(mockExplorerTreeItemA.Object);

            var mockExplorerTreeItemC = new Mock<IExplorerTreeItem>();
            mockExplorerTreeItemC.Setup(item => item.ResourceType).Returns("Folder");
            mockExplorerTreeItemC.Setup(item => item.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            mockExplorerTreeItemC.Setup(item => item.ResourceName).Returns("C");
            mockExplorerTreeItemC.Setup(item => item.Parent).Returns(mockExplorerTreeItemB.Object);

            var mockExplorerParentTreeItemD = new Mock<IExplorerTreeItem>();
            mockExplorerParentTreeItemD.Setup(item => item.ResourceType).Returns("Folder");
            mockExplorerParentTreeItemD.Setup(item => item.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            mockExplorerParentTreeItemD.Setup(item => item.ResourceName).Returns("D");
            mockExplorerParentTreeItemD.Setup(item => item.Parent).Returns(mockExplorerTreeItemC.Object);

            requestServiceNameViewModel.SingleEnvironmentExplorerViewModel.SelectedItem = mockExplorerParentTreeItemD.Object;
            //------------Execute Test---------------------------
            requestServiceNameViewModel.OkCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsNotNull(requestServiceNameViewModel.ResourceName);
            Assert.AreEqual("A\\B\\C\\D\\", requestServiceNameViewModel.ResourceName.Path);
        }


        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_ShowSaveDialog_ItemSelectedHasDuplicateName_ShouldReturnError()
        {
            //------------Setup for test--------------------------
            const string expectedErrorMessage = "An item with this name already exists in this folder.";
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();


            var mockExplorerTreeItem = new Mock<IExplorerTreeItem>();
            mockExplorerTreeItem.Setup(item => item.ResourceType).Returns("Folder");

            mockExplorerTreeItem.Setup(item => item.ResourceName).Returns("MyFolder");

            var childDuplicateExplorerTreeItem = new Mock<IExplorerItemViewModel>();
            childDuplicateExplorerTreeItem.Setup(item => item.ResourceType).Returns("DbService");
            childDuplicateExplorerTreeItem.Setup(item => item.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            childDuplicateExplorerTreeItem.Setup(item => item.ResourceName).Returns("TestResource");
            childDuplicateExplorerTreeItem.Setup(model => model.Parent).Returns(mockExplorerTreeItem.Object);

            var explorerItemViewModels = new ObservableCollection<IExplorerItemViewModel> { childDuplicateExplorerTreeItem.Object };
            mockExplorerTreeItem.Setup(item => item.Children).Returns(explorerItemViewModels);
            requestServiceNameViewModel.SingleEnvironmentExplorerViewModel.SelectedItem = mockExplorerTreeItem.Object;
            requestServiceNameViewModel.Name = "TestResource";
            //------------Execute Test---------------------------
            requestServiceNameViewModel.OkCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.AreEqual(expectedErrorMessage, requestServiceNameViewModel.ErrorMessage);
            Assert.IsFalse(requestServiceNameViewModel.OkCommand.CanExecute(null));
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_ShowSaveDialog_ItemSelectedHasDuplicateName_WhenDuplicate_ShouldReturnError()
        {
            //------------Setup for test--------------------------
            const string expectedErrorMessage = "An item with this name already exists in this folder.";

            var mockExplorerTreeItem = new Mock<IExplorerItemViewModel>();
            mockExplorerTreeItem.Setup(item => item.ResourceType).Returns("Folder");
            mockExplorerTreeItem.Setup(item => item.ResourceName).Returns("MyFolder");
            var childDuplicateExplorerTreeItem = new Mock<IExplorerItemViewModel>();
            childDuplicateExplorerTreeItem.Setup(item => item.ResourceType).Returns("DbService");
            childDuplicateExplorerTreeItem.Setup(item => item.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            childDuplicateExplorerTreeItem.Setup(item => item.ResourceName).Returns("TestResource");
            childDuplicateExplorerTreeItem.Setup(model => model.Parent).Returns(mockExplorerTreeItem.Object);
            var explorerItemViewModels = new ObservableCollection<IExplorerItemViewModel> { childDuplicateExplorerTreeItem.Object };
            mockExplorerTreeItem.Setup(item => item.Children).Returns(explorerItemViewModels);

            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            mockEnvironmentModel.Setup(model => model.Children).Returns(new AsyncObservableCollection<IExplorerItemViewModel> { mockExplorerTreeItem.Object });
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "", mockExplorerTreeItem.Object);
            requestServiceNameViewModel.ShowSaveDialog();

            requestServiceNameViewModel.SingleEnvironmentExplorerViewModel.SelectedItem = null;
            //------------Execute Test---------------------------
            requestServiceNameViewModel.Name = "MyFolder";
            //------------Assert Results-------------------------
            Assert.IsTrue(requestServiceNameViewModel.IsDuplicate);
            Assert.AreEqual(expectedErrorMessage, requestServiceNameViewModel.ErrorMessage);
            Assert.IsFalse(requestServiceNameViewModel.OkCommand.CanExecute(null));
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_ShowSaveDialog_NoItemSelectedHasDuplicateName_ShouldReturnError()
        {
            //------------Setup for test--------------------------
            const string expectedErrorMessage = "An item with this name already exists in this folder.";
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            var mockExplorerTreeItem = new Mock<IExplorerTreeItem>();
            mockExplorerTreeItem.Setup(item => item.ResourceType).Returns("Folder");
            mockExplorerTreeItem.Setup(item => item.ResourceName).Returns("MyFolder");
            var childDuplicateExplorerTreeItem = new Mock<IExplorerItemViewModel>();
            childDuplicateExplorerTreeItem.Setup(item => item.ResourceType).Returns("DbService");
            childDuplicateExplorerTreeItem.Setup(item => item.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            childDuplicateExplorerTreeItem.Setup(item => item.ResourceName).Returns("TestResource");
            childDuplicateExplorerTreeItem.Setup(model => model.Parent).Returns(mockExplorerTreeItem.Object);
            var explorerItemViewModels = new ObservableCollection<IExplorerItemViewModel> { childDuplicateExplorerTreeItem.Object };
            mockEnvironmentModel.Setup(model => model.Children).Returns(explorerItemViewModels);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();
            requestServiceNameViewModel.Name = "TestResource";
            //------------Execute Test---------------------------
            requestServiceNameViewModel.OkCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.AreEqual(expectedErrorMessage, requestServiceNameViewModel.ErrorMessage);
            Assert.IsFalse(requestServiceNameViewModel.OkCommand.CanExecute(null));
        }


        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_ShowSaveDialog_NameEmpty_ShouldHaveErrorMessage()
        {
            //------------Setup for test--------------------------
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();
            requestServiceNameViewModel.Name = "";
            //------------Execute Test---------------------------
            requestServiceNameViewModel.OkCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.AreEqual(Resource.Errors.ErrorResource.CannotBeNull, requestServiceNameViewModel.ErrorMessage);
            Assert.IsFalse(requestServiceNameViewModel.OkCommand.CanExecute(null));
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_ShowSaveDialog_NameContainsInvalidCharacters_ShouldHaveErrorMessage()
        {
            //------------Setup for test--------------------------
            const string expectedErrorMessage = "'Name' contains invalid characters";
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();
            requestServiceNameViewModel.Name = "Save@#$";
            //------------Execute Test---------------------------
            requestServiceNameViewModel.OkCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.AreEqual(expectedErrorMessage, requestServiceNameViewModel.ErrorMessage);
            Assert.IsFalse(requestServiceNameViewModel.OkCommand.CanExecute(null));
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_ShowSaveDialog_NameContainsLeadingTrailingSpaces_ShouldHaveErrorMessage()
        {
            //------------Setup for test--------------------------
            const string expectedErrorMessage = "'Name' contains leading or trailing whitespace characters.";
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();
            requestServiceNameViewModel.Name = " Save ";
            //------------Execute Test---------------------------
            requestServiceNameViewModel.OkCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.AreEqual(expectedErrorMessage, requestServiceNameViewModel.ErrorMessage);
            Assert.IsFalse(requestServiceNameViewModel.OkCommand.CanExecute(null));
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_ShowSaveDialog_NameValidNotLoaded_CanClickOk()
        {
            //------------Setup for test--------------------------
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            mockEnvironmentModel.Setup(model => model.LoadDialogAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();
            requestServiceNameViewModel.Name = "TesResource";
            //------------Execute Test---------------------------
            requestServiceNameViewModel.OkCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.AreEqual("", requestServiceNameViewModel.ErrorMessage);
            var canExecute = requestServiceNameViewModel.OkCommand.CanExecute(null);
            Assert.IsTrue(canExecute);
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_Header_Set_ShouldFirePropertyChangedEvent()
        {
            //------------Setup for test--------------------------
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            var called = false;
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            mockEnvironmentModel.Setup(model => model.LoadDialogAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "") as RequestServiceNameViewModel;
            Assert.IsNotNull(requestServiceNameViewModel);
            requestServiceNameViewModel.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "Header")
                    {
                        called = true;
                    }
                };
            //------------Execute Test---------------------------
            requestServiceNameViewModel.Header = "TestHeader";
            //------------Assert Results-------------------------
            Assert.IsTrue(called);
            Assert.AreEqual("TestHeader", requestServiceNameViewModel.Header);
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_CancelCommand_Called_ShouldCloseView()
        {
            //------------Setup for test--------------------------
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            mockRequestServiceNameView.Setup(view => view.RequestClose()).Verifiable();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            mockEnvironmentModel.Setup(model => model.LoadDialogAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();
            //------------Execute Test---------------------------
            requestServiceNameViewModel.CancelCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsNull(requestServiceNameViewModel.SingleEnvironmentExplorerViewModel);
            mockRequestServiceNameView.Verify(view => view.RequestClose());
        }


        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_SetName_SingleEnvironmentModelNull_ErrorMessageEmpty()
        {
            //------------Setup for test--------------------------
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            mockRequestServiceNameView.Setup(view => view.RequestClose()).Verifiable();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            //------------Execute Test---------------------------
            requestServiceNameViewModel.Name = "Test";
            //------------Assert Results-------------------------
            Assert.AreEqual("", requestServiceNameViewModel.ErrorMessage);
        }

        [TestMethod, Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_SetName_SingleEnvironmentModelEnvironmentsNull_ErrorMessageEmpty()
        {
            //------------Setup for test--------------------------
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            mockRequestServiceNameView.Setup(view => view.RequestClose()).Verifiable();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");
            requestServiceNameViewModel.ShowSaveDialog();
            requestServiceNameViewModel.SingleEnvironmentExplorerViewModel.Environments = new ObservableCollection<IEnvironmentViewModel>();
            //------------Execute Test---------------------------
            requestServiceNameViewModel.Name = "Test";
            //------------Assert Results-------------------------
            Assert.AreEqual("", requestServiceNameViewModel.ErrorMessage);
        }

        [TestMethod, Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public async Task RequestServiceNameViewModel_HasLoadedFalse_CanDuplicateFalse()
        {
            //------------Setup for test--------------------------
            var mockRequestServiceNameView = new Mock<IRequestServiceNameView>();
            mockRequestServiceNameView.Setup(view => view.RequestClose()).Verifiable();
            CustomContainer.RegisterInstancePerRequestType<IRequestServiceNameView>(() => mockRequestServiceNameView.Object);
            var mockEnvironmentModel = new Mock<IEnvironmentViewModel>();
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var requestServiceNameViewModel = await RequestServiceNameViewModel.CreateAsync(mockEnvironmentModel.Object, "", "");

            var fieldInfo = typeof(RequestServiceNameViewModel).GetField("_selectedPath", BindingFlags.NonPublic | BindingFlags.Instance);

            fieldInfo.SetValue(requestServiceNameViewModel, "Hello World");
            requestServiceNameViewModel.ShowSaveDialog();
            requestServiceNameViewModel.SingleEnvironmentExplorerViewModel.Environments = new ObservableCollection<IEnvironmentViewModel>();
            //------------Execute Test---------------------------
            requestServiceNameViewModel.Name = "Test";
            //------------Assert Results-------------------------
            Assert.AreEqual("", requestServiceNameViewModel.ErrorMessage);
            var canExecute = requestServiceNameViewModel.DuplicateCommand.CanExecute(null);
            Assert.IsFalse(canExecute);
        }

        [TestMethod, Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public void FixReferences_GivenIsNew_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            var viewModel = new RequestServiceNameViewModel();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(viewModel);
            //---------------Execute Test ----------------------
            var fixReferences = viewModel.FixReferences;
            //---------------Test Result -----------------------
            Assert.IsFalse(fixReferences);
            viewModel.FixReferences = true;
            Assert.IsTrue(viewModel.FixReferences);
        }

        [TestMethod, Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public void CallDuplicateService_GivenValidComsController_ShouldExecuteCorrectly()
        {
            //---------------Set up test pack-------------------

            var envMock = new Mock<IEnvironmentConnection>();
            var controller = new Mock<ICommunicationController>();
            var envModel = new Mock<IEnvironmentViewModel>();
            envModel.Setup(model => model.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            var itemObj = new Mock<IExplorerItemViewModel>();
            var selectedItemMock = new Mock<IExplorerViewModel>();
            var item = new Mock<IExplorerTreeItem>();
            item.Setup(model => model.ResourceName).Returns("name");
            item.Setup(model => model.ResourceType).Returns("type");
            item.Setup(model => model.ResourceName).Returns("name");
            selectedItemMock.Setup(sitem => sitem.SelectedItem).Returns(item.Object);
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var viewModel = RequestServiceNameViewModel.CreateAsync(envModel.Object, "", "", itemObj.Object).Result;

            controller.Setup(communicationController => communicationController.AddPayloadArgument("ResourceID", It.IsAny<string>()));
            controller.Setup(communicationController => communicationController.AddPayloadArgument("NewResourceName", It.IsAny<string>()));
            controller.Setup(communicationController => communicationController.AddPayloadArgument("FixRefs", It.IsAny<string>()));
            controller.Setup(communicationController => communicationController.ExecuteCommand<ExecuteMessage>(It.IsAny<IEnvironmentConnection>(), It.IsAny<Guid>()));
            var lazyCon = typeof(RequestServiceNameViewModel).GetField("_lazyCon", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);

            lazyCon.SetValue(viewModel, envMock.Object);
            var lazyComs = typeof(RequestServiceNameViewModel).GetField("_communicationController", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);

            lazyComs.SetValue(viewModel, controller.Object);
            var selectedItem = typeof(RequestServiceNameViewModel).GetProperty("SingleEnvironmentExplorerViewModel", BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);

            selectedItem.SetValue(viewModel, selectedItemMock.Object);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(viewModel);
            //---------------Execute Test ----------------------

            try
            {
                viewModel.DuplicateCommand.Execute(null);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            //---------------Test Result -----------------------
            controller.Verify(communicationController => communicationController.AddPayloadArgument("ResourceID", It.IsAny<string>()));
            controller.Verify(communicationController => communicationController.AddPayloadArgument("NewResourceName", It.IsAny<string>()));
            controller.Verify(communicationController => communicationController.ExecuteCommandAsync<ResourceCatalogDuplicateResult>(It.IsAny<IEnvironmentConnection>(), It.IsAny<Guid>()));
        }

        [TestMethod, Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public void RequestServiceNameViewModel_CallDuplicateCommand_GivenNoItemPassed_ShouldSetCanExecuteFalse()
        {
            //---------------Set up test pack-------------------

            var envMock = new Mock<IEnvironmentConnection>();
            var controller = new Mock<ICommunicationController>();
            var envModel = new Mock<IEnvironmentViewModel>();
            var selectedItemMock = new Mock<IExplorerViewModel>();
            var item = new Mock<IExplorerTreeItem>();
            item.Setup(model => model.ResourceName).Returns("name");
            item.Setup(model => model.ResourceType).Returns("type");
            item.Setup(model => model.ResourceName).Returns("name");
            selectedItemMock.Setup(sitem => sitem.SelectedItem).Returns(item.Object);
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var viewModel = RequestServiceNameViewModel.CreateAsync(envModel.Object, "", "").Result;

            controller.Setup(communicationController => communicationController.AddPayloadArgument("ResourceID", It.IsAny<string>()));
            controller.Setup(communicationController => communicationController.AddPayloadArgument("NewResourceName", It.IsAny<string>()));
            controller.Setup(communicationController => communicationController.AddPayloadArgument("FixRefs", It.IsAny<string>()));
            controller.Setup(communicationController => communicationController.ExecuteCommand<ExecuteMessage>(It.IsAny<IEnvironmentConnection>(), It.IsAny<Guid>()));
            var lazyCon = typeof(RequestServiceNameViewModel).GetField("_lazyCon", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);

            lazyCon.SetValue(viewModel, envMock.Object);
            var lazyComs = typeof(RequestServiceNameViewModel).GetField("_communicationController", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);

            lazyComs.SetValue(viewModel, controller.Object);
            var selectedItem = typeof(RequestServiceNameViewModel).GetProperty("SingleEnvironmentExplorerViewModel", BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);

            selectedItem.SetValue(viewModel, selectedItemMock.Object);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(viewModel);
            //---------------Execute Test ----------------------

            var canExecute = viewModel.DuplicateCommand.CanExecute(null);
            //---------------Test Result -----------------------
            Assert.IsFalse(canExecute);
        }


        [TestMethod, Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        [TestCategory(nameof(RequestServiceNameViewModel))]
        public void RequestServiceNameViewModel_CallDuplicateCommand_GivenIsFolder_ShouldAddValidPayloads()
        {
            //---------------Set up test pack-------------------

            var envMock = new Mock<IEnvironmentConnection>();
            var controller = new Mock<ICommunicationController>();
            var envModel = new Mock<IEnvironmentViewModel>();
            envModel.Setup(model => model.Children).Returns(new ObservableCollection<IExplorerItemViewModel>());
            var selectedItemMock = new Mock<IExplorerViewModel>();
            var itemObj = new Mock<IExplorerItemViewModel>();
            itemObj.Setup(model => model.IsFolder).Returns(true);
            var item = new Mock<IExplorerTreeItem>();
            item.Setup(model => model.ResourceName).Returns("name");
            item.Setup(model => model.ResourceType).Returns("type");
            item.Setup(model => model.ResourceName).Returns("name");
            item.Setup(model => model.IsFolder).Returns(true);
            selectedItemMock.Setup(sitem => sitem.SelectedItem).Returns(item.Object);
            var serverRepo = new Mock<IServerRepository>();
            var connectionObject = new Mock<IEnvironmentConnection>();
            serverRepo.Setup(repository => repository.ActiveServer.Connection).Returns(connectionObject.Object);
            CustomContainer.Register(serverRepo.Object);
            var viewModel = RequestServiceNameViewModel.CreateAsync(envModel.Object, "", "", itemObj.Object).Result;

            controller.Setup(communicationController => communicationController.AddPayloadArgument("ResourceID", It.IsAny<string>()));
            controller.Setup(communicationController => communicationController.AddPayloadArgument("NewResourceName", It.IsAny<string>()));
            controller.Setup(communicationController => communicationController.AddPayloadArgument("FixRefs", It.IsAny<string>()));
            controller.Setup(communicationController => communicationController.ExecuteCommand<ExecuteMessage>(It.IsAny<IEnvironmentConnection>(), It.IsAny<Guid>()));
            var lazyCon = typeof(RequestServiceNameViewModel).GetField("_lazyCon", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);

            lazyCon.SetValue(viewModel, envMock.Object);
            var lazyComs = typeof(RequestServiceNameViewModel).GetField("_communicationController", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);

            lazyComs.SetValue(viewModel, controller.Object);
            var selectedItem = typeof(RequestServiceNameViewModel).GetProperty("SingleEnvironmentExplorerViewModel", BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);

            selectedItem.SetValue(viewModel, selectedItemMock.Object);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(viewModel);


            //---------------Execute Test ----------------------
            var canExecute = viewModel.DuplicateCommand.CanExecute(null);
            try
            {
                viewModel.DuplicateCommand.Execute(null);

            }
            catch (Exception f) when (f is NullReferenceException)
            {
                Console.WriteLine(f);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //---------------Test Result -----------------------
            var value = lazyComs.GetValue(viewModel) as ICommunicationController;
            Assert.IsNotNull(value);
            Assert.AreEqual("DuplicateFolderService", value.ServiceName);

        }
    }
}