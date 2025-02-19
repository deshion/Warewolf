/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Caliburn.Micro;
using CubicOrange.Windows.Forms.ActiveDirectory;
using Dev2.Common.Interfaces.Enums;
using Dev2.Common.Interfaces.Infrastructure;
using Dev2.Common.Interfaces.Scheduler.Interfaces;
using Dev2.Common.Interfaces.Studio.Controller;
using Dev2.Communication;
using Dev2.Data.TO;
using Dev2.Dialogs;
using Dev2.Scheduler;
using Dev2.Services.Security;
using Dev2.Studio.Controller;
using Dev2.Studio.Core;
using Dev2.Studio.Core.AppResources.Repositories;
using Dev2.Studio.Core.Messages;
using Dev2.Studio.Core.Models;
using Dev2.Studio.Interfaces;
using Dev2.Studio.Interfaces.Enums;
using Dev2.TaskScheduler.Wrappers;
using Dev2.Threading;
using Dev2.Triggers.Scheduler;
using Dev2.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32.TaskScheduler;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Dev2.Core.Tests.Triggers.Scheduler
{
    [TestClass]
    [TestCategory("Studio Triggers Scheduler Core")]
    public class SchedulerViewModelTests
    {
        [TestInitialize]
        public void SetupForTest()
        {
            AppUsageStats.LocalHost = "http://localhost:3142";
            var shell = new Mock<IShellViewModel>();
            var lcl = new Mock<IServer>();
            lcl.Setup(a => a.DisplayName).Returns("Localhost");
            shell.Setup(x => x.LocalhostServer).Returns(lcl.Object);
            shell.Setup(x => x.ActiveServer).Returns(new Mock<IServer>().Object);
            CustomContainer.Register(shell.Object);
            CustomContainer.Register(new Mock<Microsoft.Practices.Prism.PubSubEvents.IEventAggregator>().Object);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_Constructor")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchedulerViewModel_Constructor_NullEventAggregator_Exception()
        {
            //------------Setup for test--------------------------
            //------------Execute Test---------------------------

            new SchedulerViewModel(null, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);

            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_Constructor")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchedulerViewModel_Constructor_NullDirectoryObjectPickerDialog_Exception()
        {
            //------------Setup for test--------------------------
            //------------Execute Test---------------------------

            new SchedulerViewModel(new Mock<IEventAggregator>().Object, null, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);

            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_Constructor")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchedulerViewModel_Constructor_NullPopupController_Exception()
        {
            //------------Setup for test--------------------------
            //------------Execute Test---------------------------

            new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, null, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);

            //------------Assert Results-------------------------
        }
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_Constructor")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchedulerViewModel_Constructor_NullAsyncWorker_Exception()
        {
            //------------Setup for test--------------------------
            //------------Execute Test---------------------------

            new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, null, new Mock<IServer>().Object, a => new Mock<IServer>().Object);

            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_Constructor")]
        public void SchedulerViewModel_Constructor_ValidConstruction_ShouldSetProperties()
        {
            //------------Setup for test--------------------------

            //------------Execute Test---------------------------
            var schdulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object);
            //------------Assert Results-------------------------
            Assert.IsNotNull(schdulerViewModel);
            Assert.IsNotNull(schdulerViewModel.Errors);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SchedulerViewModel_Constructor")]
        public void SchedulerViewModel_Constructor_ValidConstruction_ShouldSetErrors()
        {
            //------------Setup for test--------------------------

            //------------Execute Test---------------------------
            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object);
            var scheduledResourceForTest = new ScheduledResourceForTest();
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            schedulerViewModel.SelectedTask.Errors = new ErrorResultTO();
            //------------Assert Results-------------------------
            Assert.IsNotNull(schedulerViewModel);
            Assert.IsNotNull(schedulerViewModel.Errors);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SchedulerViewModel_HasDebugOutput")]
        public void SchedulerViewModel_HasDebugOutput_ExpectedTrue()
        {
            //------------Setup for test--------------------------

            //------------Execute Test---------------------------
            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object);
            var scheduledResourceForTest = new ScheduledResourceForTest();
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            schedulerViewModel.SelectedTask.Errors = new ErrorResultTO();
            //------------Assert Results-------------------------
            Assert.IsNotNull(schedulerViewModel);
            Assert.IsTrue(schedulerViewModel.HasDebugOutput);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SchedulerViewModel_Constructor")]
        public void SchedulerViewModel_Constructor_SetDisplayName_OnlyForCoverage()
        {
            //------------Setup for test--------------------------

            //------------Execute Test---------------------------
            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object);
            schedulerViewModel.DisplayName = "displayName";
            //------------Assert Results-------------------------
            Assert.AreEqual("displayName", schedulerViewModel.DisplayName);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SchedulerViewModel_Constructor")]
        public void SchedulerViewModel_Constructor_ServerNotNull_ShouldSetDisplayName()
        {
            //------------Setup for test--------------------------

            //------------Execute Test---------------------------
            var schdulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object);
            //------------Assert Results-------------------------
            Assert.IsNotNull(schdulerViewModel);
            Assert.IsNotNull(schdulerViewModel.Errors);
            Assert.AreEqual("Scheduler - ", schdulerViewModel.DisplayName);
        }

        [TestMethod]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("SchedulerViewModel_Constructor")]
        public void SchedulerViewModel_Constructor_ValidConstruction_ShouldSetHelpText()
        {
            //------------Setup for test--------------------------

            //------------Execute Test---------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var expectedHelpText = Warewolf.Studio.Resources.Languages.HelpText.SchedulerSettingsHelpTextSettingsView;

            //------------Assert Results-------------------------
            Assert.IsNotNull(schedulerViewModel.HelpToggle);
            Assert.AreEqual(expectedHelpText, schedulerViewModel.HelpText);

            schedulerViewModel.HelpText = expectedHelpText;
            Assert.AreEqual(expectedHelpText, schedulerViewModel.HelpText);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_ShowError")]
        public void SchedulerViewModel_ShowError_WithSaveError_HasErrorsTrue()
        {
            //------------Setup for test--------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object) { SelectedTask = new ScheduledResourceForTest() };

            //------------Execute Test---------------------------
            schedulerViewModel.ShowError("Error while saving: test error");
            //------------Assert Results-------------------------
            Assert.IsTrue(schedulerViewModel.HasErrors);
            Assert.IsFalse(schedulerViewModel.IsLoading);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_ShowError")]
        public void SchedulerViewModel_ShowError_WithNormalError_HasErrorsTrue()
        {
            //------------Setup for test--------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object) { SelectedTask = new ScheduledResourceForTest() };
            var _hasErrorChange = false;
            var _errorChange = false;

            schedulerViewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case "HasErrors":
                        _hasErrorChange = true;
                        break;
                    case "Error":
                        _errorChange = true;
                        break;
                    default:
                        break;
                }
            };

            //------------Execute Test---------------------------
            schedulerViewModel.ShowError("test error");
            //------------Assert Results-------------------------
            Assert.IsTrue(_hasErrorChange);
            Assert.IsTrue(_errorChange);
            Assert.IsTrue(schedulerViewModel.HasErrors);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_ClearError")]
        public void SchedulerViewModel_ClearError_WithNormalError_HasErrorsSet()
        {
            //------------Setup for test--------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object) { SelectedTask = new ScheduledResourceForTest() };
            var _hasErrorChange = false;
            var _errorChange = false;

            schedulerViewModel.ShowError("test error");

            Assert.IsTrue(schedulerViewModel.HasErrors);

            schedulerViewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case "HasErrors":
                        _hasErrorChange = true;
                        break;
                    case "Error":
                        _errorChange = true;
                        break;
                    default:
                        break;
                }
            };

            //------------Execute Test---------------------------
            schedulerViewModel.ClearError("test error");
            //------------Assert Results-------------------------
            Assert.IsTrue(_hasErrorChange);
            Assert.IsTrue(_errorChange);
            Assert.IsFalse(schedulerViewModel.HasErrors);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_Trigger")]
        public void SchedulerViewModel_Trigger_SetTrigger_IsDirtyTrue()
        {
            //------------Setup for test--------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var scheduleTrigger = new ScheduleTrigger(TaskState.Ready, new Dev2DailyTrigger(new TaskServiceConvertorFactory(), new DailyTrigger()), new Dev2TaskService(new TaskServiceConvertorFactory()), new TaskServiceConvertorFactory());
            schedulerViewModel.Trigger = scheduleTrigger;
            var scheduleResource = new ScheduledResource("Task", SchedulerStatus.Disabled, DateTime.Now, scheduleTrigger, "TestWf", Guid.NewGuid().ToString());
            schedulerViewModel.SelectedTask = scheduleResource;
            //------------Execute Test---------------------------
            Assert.IsFalse(schedulerViewModel.SelectedTask.IsDirty);
            var newScheduleTrigger = new ScheduleTrigger(TaskState.Queued, new Dev2DailyTrigger(new TaskServiceConvertorFactory(), new DailyTrigger()), new Dev2TaskService(new TaskServiceConvertorFactory()), new TaskServiceConvertorFactory());
            schedulerViewModel.Trigger = newScheduleTrigger;
            //------------Assert Results-------------------------
            Assert.IsNotNull(schedulerViewModel.Trigger);
            Assert.IsTrue(schedulerViewModel.IsDirty);
            Assert.IsTrue(schedulerViewModel.SelectedTask.IsDirty);
        }


        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_Trigger")]
        public void SchedulerViewModel_SaveCommand_AfterSave_IsDirtyFalse()
        {
            var explorerTooltips = new Mock<IExplorerTooltips>();
            CustomContainer.Register(explorerTooltips.Object);
            var serverRepository = new Mock<IServerRepository>();
            CustomContainer.Register(serverRepository.Object);
            //------------Setup for test--------------------------
            var scheduleTrigger = new ScheduleTrigger(TaskState.Ready, new Dev2DailyTrigger(new TaskServiceConvertorFactory(), new DailyTrigger()), new Dev2TaskService(new TaskServiceConvertorFactory()), new TaskServiceConvertorFactory());
            var scheduleResource = new ScheduledResource("Task", SchedulerStatus.Disabled, DateTime.Now, scheduleTrigger, "TestWf", Guid.NewGuid().ToString());
            var resources = new ObservableCollection<IScheduledResource>();
            resources.Add(scheduleResource);
            scheduleResource.UserName = "some user";
            scheduleResource.Password = "some password";

            var env = new Mock<IServer>();
            var schedulerViewModel = new SchedulerViewModelForTest(env.Object);
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();
            schedulerViewModel.CurrentEnvironment = env.Object;
            schedulerViewModel.ToEnvironmentModel = a => env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            string errorMessage;
            mockScheduledResourceModel.Setup(model => model.Save(It.IsAny<IScheduledResource>(), out errorMessage)).Returns(true);
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            schedulerViewModel.Trigger = scheduleTrigger;
            schedulerViewModel.SelectedTask = scheduleResource;
            //------------Assert Preconditions-------------------
            Assert.IsFalse(schedulerViewModel.SelectedTask.IsDirty);
            var newScheduleTrigger = new ScheduleTrigger(TaskState.Queued, new Dev2DailyTrigger(new TaskServiceConvertorFactory(), new DailyTrigger()), new Dev2TaskService(new TaskServiceConvertorFactory()), new TaskServiceConvertorFactory());
            schedulerViewModel.Trigger = newScheduleTrigger;
            Assert.IsNotNull(schedulerViewModel.Trigger);
            Assert.IsTrue(schedulerViewModel.IsDirty);
            Assert.IsTrue(schedulerViewModel.SelectedTask.IsDirty);
            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsFalse(schedulerViewModel.IsDirty);
            Assert.IsFalse(schedulerViewModel.SelectedTask.IsDirty);

        }


        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SchedulerViewModel_Trigger")]
        public void SchedulerViewModel_Trigger_SetTrigger_IsDirtyFalse()
        {
            //------------Setup for test--------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var scheduleTrigger = new ScheduleTrigger(TaskState.Ready, new Dev2DailyTrigger(new TaskServiceConvertorFactory(), new DailyTrigger()), new Dev2TaskService(new TaskServiceConvertorFactory()), new TaskServiceConvertorFactory());
            schedulerViewModel.Trigger = scheduleTrigger;
            var scheduleResource = new ScheduledResource("Task", SchedulerStatus.Disabled, DateTime.Now, scheduleTrigger, "TestWf", Guid.NewGuid().ToString());

            schedulerViewModel.SelectedTask = scheduleResource;
            //------------Execute Test---------------------------
            Assert.IsFalse(schedulerViewModel.SelectedTask.IsDirty);
            Assert.IsFalse(schedulerViewModel.IsDirty);
            //------------Assert Results-------------------------
            schedulerViewModel.Status = SchedulerStatus.Enabled;
            Assert.IsTrue(schedulerViewModel.IsDirty);
            Assert.IsTrue(schedulerViewModel.SelectedTask.IsDirty);

            schedulerViewModel.Status = SchedulerStatus.Disabled;
            Assert.IsFalse(schedulerViewModel.IsDirty);
            Assert.IsFalse(schedulerViewModel.SelectedTask.IsDirty);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_RunAsapIfScheduleMissed")]
        public void SchedulerViewModel_RunAsapIfScheduleMissed_SetRunAsapIfScheduleMissed_IsDirtyTrue()
        {
            //------------Setup for test--------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var scheduledResourceForTest = new ScheduledResourceForTest();
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            //------------Execute Test---------------------------
            Assert.IsFalse(schedulerViewModel.SelectedTask.IsDirty);
            schedulerViewModel.RunAsapIfScheduleMissed = true;
            //------------Assert Results-------------------------
            Assert.IsTrue(schedulerViewModel.RunAsapIfScheduleMissed);
            schedulerViewModel.RunAsapIfScheduleMissed = true;
            Assert.IsTrue(schedulerViewModel.RunAsapIfScheduleMissed);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_Status")]
        public void SchedulerViewModel_Status_SetStatus_IsDirtyTrue()
        {
            //------------Setup for test--------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var scheduledResourceForTest = new ScheduledResourceForTest();
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            //------------Execute Test---------------------------
            Assert.IsFalse(schedulerViewModel.SelectedTask.IsDirty);
            schedulerViewModel.Status = SchedulerStatus.Disabled;
            //------------Assert Results-------------------------
            Assert.AreEqual(SchedulerStatus.Disabled, schedulerViewModel.Status);

            schedulerViewModel.Status = SchedulerStatus.Disabled;
            Assert.AreEqual(SchedulerStatus.Disabled, schedulerViewModel.Status);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_NumberOfRecordsToKeep")]
        public void SchedulerViewModel_NumberOfRecordsToKeep_SetNumberOfRecordsToKeepToBlank_ValueIsZero()
        {
            //------------Setup for test--------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var scheduledResourceForTest = new ScheduledResourceForTest();
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            //------------Execute Test---------------------------
            schedulerViewModel.NumberOfRecordsToKeep = "";
            //------------Assert Results-------------------------
            Assert.AreEqual("0", schedulerViewModel.NumberOfRecordsToKeep);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_NumberOfRecordsToKeep")]
        public void SchedulerViewModel_NumberOfRecordsToKeep_SetNumberOfRecordsToKeepToNoNumeric_ValueIsZero()
        {
            //------------Setup for test--------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var scheduledResourceForTest = new ScheduledResourceForTest();
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            //------------Execute Test---------------------------
            schedulerViewModel.NumberOfRecordsToKeep = "asdasd";
            //------------Assert Results-------------------------
            Assert.AreEqual("0", schedulerViewModel.NumberOfRecordsToKeep);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_SelectedHistory")]
        public void SchedulerViewModel_SelectedHistory_SetSelectedHistory_DebugMessageFiredTwice()
        {
            //------------Setup for test--------------------------
            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(c => c.Publish(It.IsAny<DebugOutputMessage>())).Verifiable();
            var schedulerViewModel = new SchedulerViewModel(eventAggregator.Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var scheduledResourceForTest = new ScheduledResourceForTest();
            schedulerViewModel.SelectedTask = scheduledResourceForTest; //Fires DebugOutMessage for null SelectedHistory
            //------------Execute Test---------------------------
            var selectedHistory = new ResourceHistoryForTest();
            schedulerViewModel.SelectedHistory = selectedHistory; //Fires DebugOutMessage for set SelectedHistory
            //------------Assert Results-------------------------
            Assert.IsNotNull(schedulerViewModel.SelectedHistory);
            eventAggregator.Verify(c => c.Publish(It.IsAny<DebugOutputMessage>()), Times.Exactly(1));

            schedulerViewModel.SelectedHistory = selectedHistory;
            Assert.IsNotNull(schedulerViewModel.SelectedHistory);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_AccountName")]
        public void SchedulerViewModel_AccountName_SetAccountName_IsDirty()
        {
            //------------Setup for test--------------------------
            var _accountNameChanged = false;
            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(c => c.Publish(It.IsAny<DebugOutputMessage>())).Verifiable();
            var schedulerViewModel = new SchedulerViewModel(eventAggregator.Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var scheduledResourceForTest = new ScheduledResourceForTest();
            schedulerViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "AccountName")
                {
                    _accountNameChanged = true;
                }
            };
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            schedulerViewModel.ShowError(Warewolf.Studio.Resources.Languages.Core.SchedulerLoginErrorMessage);
            Assert.AreEqual("Scheduler", schedulerViewModel.ResourceType);
            Assert.AreEqual(Warewolf.Studio.Resources.Languages.Core.SchedulerLoginErrorMessage, schedulerViewModel.Error);
            //------------Execute Test---------------------------
            schedulerViewModel.AccountName = "someAccountName";
            //------------Assert Results-------------------------
            Assert.AreEqual("someAccountName", schedulerViewModel.AccountName);
            Assert.AreEqual("", schedulerViewModel.Error);
            Assert.AreEqual("someAccountName", schedulerViewModel.SelectedTask.UserName);
            Assert.IsTrue(_accountNameChanged);

            schedulerViewModel.AccountName = "someAccountName";
            Assert.AreEqual("someAccountName", schedulerViewModel.AccountName);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_AccountName")]
        public void SchedulerViewModel_AccountName_SetAccountName_SelectedTaskNull_NothingChangedOnTask()
        {
            //------------Setup for test--------------------------
            var _accountNameChanged = false;
            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(c => c.Publish(It.IsAny<DebugOutputMessage>())).Verifiable();
            var schedulerViewModel = new SchedulerViewModel(eventAggregator.Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var scheduledResourceForTest = new ScheduledResourceForTest();
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            schedulerViewModel.ShowError(Warewolf.Studio.Resources.Languages.Core.SchedulerLoginErrorMessage);
            Assert.AreEqual(Warewolf.Studio.Resources.Languages.Core.SchedulerLoginErrorMessage, schedulerViewModel.Error);
            schedulerViewModel.AccountName = "someAccountName";
            schedulerViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "AccountName")
                {
                    _accountNameChanged = true;
                }
            };
            //--------------Assert Preconditions------------------
            Assert.AreEqual("", schedulerViewModel.Error);
            var scheduledResource = schedulerViewModel.SelectedTask;
            Assert.AreEqual("someAccountName", scheduledResource.UserName);
            //------------Execute Test---------------------------
            schedulerViewModel.SelectedTask = null;
            schedulerViewModel.AccountName = "another account name";
            //------------Assert Results-------------------------
            Assert.IsNull(schedulerViewModel.SelectedTask);
            Assert.AreEqual("someAccountName", scheduledResource.UserName);
            Assert.IsFalse(_accountNameChanged);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_Password")]
        public void SchedulerViewModel_Password_SetPassword_IsDirty()
        {
            //------------Setup for test--------------------------
            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(c => c.Publish(It.IsAny<DebugOutputMessage>())).Verifiable();
            var schedulerViewModel = new SchedulerViewModel(eventAggregator.Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var scheduledResourceForTest = new ScheduledResourceForTest();
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            schedulerViewModel.ShowError(Warewolf.Studio.Resources.Languages.Core.SchedulerLoginErrorMessage);
            Assert.AreEqual(Warewolf.Studio.Resources.Languages.Core.SchedulerLoginErrorMessage, schedulerViewModel.Error);
            //------------Execute Test---------------------------
            schedulerViewModel.Password = "somePassword";
            //------------Assert Results-------------------------
            Assert.AreEqual("somePassword", schedulerViewModel.Password);
            Assert.AreEqual("", schedulerViewModel.Error);
            schedulerViewModel.Password = "somePassword";
            Assert.AreEqual("somePassword", schedulerViewModel.Password);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SchedulerViewModel_ConnectionError")]
        public void SchedulerViewModel_ConnectionError_SetAndClearError_ValidErrorSetAndClear()
        {
            //------------Setup for test--------------------------
            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(c => c.Publish(It.IsAny<DebugOutputMessage>())).Verifiable();
            var schedulerViewModel = new SchedulerViewModel(eventAggregator.Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);

            //------------Execute Test---------------------------
            schedulerViewModel.SetConnectionError();
            //------------Assert Results-------------------------
            Assert.AreEqual(Warewolf.Studio.Resources.Languages.Core.SchedulerConnectionError, schedulerViewModel.ConnectionError);
            Assert.IsTrue(schedulerViewModel.HasConnectionError);

            schedulerViewModel.ClearConnectionError();

            Assert.AreEqual("", schedulerViewModel.ConnectionError);
            Assert.IsFalse(schedulerViewModel.HasConnectionError);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_UserNamePasswordNotSet_CallsGetCredentials()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResourceForTest { IsDirty = true };
            resources.Add(scheduledResourceForTest);
            var schedulerViewModel = new SchedulerViewModelForTest(new Mock<IServer>().Object);
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();
            schedulerViewModel.CurrentEnvironment = env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            mockScheduledResourceModel.Setup(model => model.Save(It.IsAny<IScheduledResource>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsTrue(schedulerViewModel.GetCredentialsCalled);
            Assert.IsTrue(schedulerViewModel.ShowSaveErrorDialogCalled);
            string errorMessage;
            mockScheduledResourceModel.Verify(model => model.Save(It.IsAny<IScheduledResource>(), out errorMessage), Times.Once());
            auth.Verify(a => a.IsAuthorized(AuthorizationContext.Administrator, null));
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SchedulerViewModel_Constructor")]
        public void SchedulerViewModel_Constructor_SetupServer_Validate()
        {
            //------------Setup for test--------------------------
            CommonSetupHelper.RegisterServerRepository();
            //------------Execute Test---------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResourceForTest { IsDirty = true };
            resources.Add(scheduledResourceForTest);
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();

            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);

            var secAuth = new Mock<ISecurityService>();
            secAuth.Setup(a => a.Permissions).Returns(new List<WindowsGroupPermission> { WindowsGroupPermission.CreateAdministrators() });
            env.Setup(a => a.AuthorizationService.SecurityService).Returns(secAuth.Object);

            var mockEnvConnection = new Mock<IEnvironmentConnection>();
            mockEnvConnection.Setup(connection => connection.ServerID).Returns(Guid.NewGuid());
            mockEnvConnection.Setup(connection => connection.IsConnected).Returns(true);
            env.Setup(a => a.Connection).Returns(mockEnvConnection.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();

            var activeServer = new Server(Guid.NewGuid(), mockEnvConnection.Object)
            {
                Permissions = new List<IWindowsGroupPermission> { WindowsGroupPermission.CreateAdministrators() }
            };
            ServerRepository.Instance.ActiveServer = activeServer;

            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), activeServer, a => activeServer, System.Threading.Tasks.Task.FromResult(new Mock<IResourcePickerDialog>().Object));
            //------------Assert Results-------------------------
            Assert.IsNotNull(schedulerViewModel.CurrentEnvironment);
            Assert.IsNotNull(schedulerViewModel.CurrentEnvironment.AuthorizationService);
            Assert.IsTrue(schedulerViewModel.CurrentEnvironment.IsConnected);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_WithNewNameDiffToOldNameYesDialogResponse_DialogShowsConflict()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResource("Task2", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task1", IsDirty = true };
            scheduledResourceForTest.IsDirty = true;
            resources.Add(scheduledResourceForTest);
            resources.Add(new ScheduledResource("Task3", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task3", IsDirty = true });
            var mockPopupController = new Mock<IPopupController>();
            mockPopupController.Setup(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.Yes).Verifiable();

            var auth = new Mock<IAuthorizationService>();
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();
            var env = new Mock<IServer>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);

            var schedulerViewModel = new SchedulerViewModelForTest(env.Object, new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopupController.Object, new SynchronousAsyncWorker());
            schedulerViewModel.CurrentEnvironment = env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            mockScheduledResourceModel.Setup(model => model.Save(It.IsAny<IScheduledResource>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsTrue(schedulerViewModel.GetCredentialsCalled);
            string errorMessage;
            mockScheduledResourceModel.Verify(model => model.Save(It.IsAny<IScheduledResource>(), out errorMessage), Times.Once());
            Assert.AreEqual("Task2", schedulerViewModel.TaskList[0].Name);
            mockPopupController.Verify(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [TestMethod]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_WithNewNameDiffToOldNameYesDialogResponse_OldNameChanges()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResource("Task2", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task1", IsDirty = true };
            scheduledResourceForTest.IsDirty = true;
            resources.Add(scheduledResourceForTest);
            resources.Add(new ScheduledResource("Task3", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task3", IsDirty = true });
            var mockPopupController = new Mock<IPopupController>();
            mockPopupController.Setup(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.Yes).Verifiable();
            var auth = new Mock<IAuthorizationService>();
            var env = new Mock<IServer>();
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            env.Setup(a => a.IsConnected).Returns(true);

            var schedulerViewModel = new SchedulerViewModelForTest(env.Object, new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopupController.Object, new SynchronousAsyncWorker());

            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();
            schedulerViewModel.CurrentEnvironment = env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            string test;
            mockScheduledResourceModel.Setup(model => model.Save(It.IsAny<IScheduledResource>(), out test)).Returns(true).Verifiable();
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            schedulerViewModel.ShowError("Error while saving: test error");
            Assert.IsTrue(schedulerViewModel.HasErrors);

            var _errorsChanged = false;
            var _errorChanged = false;
            var _taskListChanged = false;


            schedulerViewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case "Errors":
                        _errorsChanged = true;
                        break;
                    case "Error":
                        _errorChanged = true;
                        break;
                    case "TaskList":
                        _taskListChanged = true;
                        break;
                    default:
                        break;
                }
            };

            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsTrue(_errorsChanged);
            Assert.IsTrue(_errorChanged);
            Assert.IsTrue(_taskListChanged);
            Assert.IsFalse(schedulerViewModel.HasErrors);
            Assert.IsTrue(schedulerViewModel.GetCredentialsCalled);
            string errorMessage;
            mockScheduledResourceModel.Verify(model => model.Save(It.IsAny<IScheduledResource>(), out errorMessage), Times.Once());
            Assert.AreEqual("Task2", schedulerViewModel.TaskList[0].Name);
            Assert.AreEqual("Task2", schedulerViewModel.TaskList[0].OldName);
            mockPopupController.Verify(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_WithNewNameDiffToOldNameYesDialogResponse_OldNameChangesNewTask()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResource("Task2", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "New Task 1", IsDirty = true };
            scheduledResourceForTest.IsDirty = true;
            resources.Add(scheduledResourceForTest);
            resources.Add(new ScheduledResource("Task3", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task3", IsDirty = true });
            var mockPopupController = new Mock<IPopupController>();
            mockPopupController.Setup(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.Yes).Verifiable();
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            env.Setup(a => a.IsConnected).Returns(true);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();

            var schedulerViewModel = new SchedulerViewModelForTest(env.Object, new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopupController.Object, new SynchronousAsyncWorker());

            schedulerViewModel.CurrentEnvironment = env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            string test;
            mockScheduledResourceModel.Setup(model => model.Save(It.IsAny<IScheduledResource>(), out test)).Returns(true).Verifiable();
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsTrue(schedulerViewModel.GetCredentialsCalled);
            string errorMessage;
            mockScheduledResourceModel.Verify(model => model.Save(It.IsAny<IScheduledResource>(), out errorMessage), Times.Once());
            Assert.AreEqual("Task2", schedulerViewModel.TaskList[0].Name);
            Assert.AreEqual("Task2", schedulerViewModel.TaskList[0].OldName);

        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_ServerNotConnected_ErrorMessageSet()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResource("Task2", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "New Task 1", IsDirty = true };
            scheduledResourceForTest.IsDirty = true;
            resources.Add(scheduledResourceForTest);
            resources.Add(new ScheduledResource("Task3", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task3", IsDirty = true });
            var mockPopupController = new Mock<IPopupController>();
            mockPopupController.Setup(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.Yes).Verifiable();
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(c => c.IsConnected).Returns(false);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(false).Verifiable();

            var schedulerViewModel = new SchedulerViewModelForTest(env.Object, new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopupController.Object, new SynchronousAsyncWorker());

            schedulerViewModel.CurrentEnvironment = env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);

            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            auth.Verify(a => a.IsAuthorized(AuthorizationContext.Administrator, null), Times.Never());
            Assert.AreEqual(schedulerViewModel.Errors.FetchErrors().Count, 1);
            Assert.AreEqual(@"Error while saving: Server unreachable.", schedulerViewModel.Errors.FetchErrors().FirstOrDefault());
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_AuthorizationFails()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResource("Task2", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "New Task 1", IsDirty = true };
            scheduledResourceForTest.IsDirty = true;
            resources.Add(scheduledResourceForTest);
            resources.Add(new ScheduledResource("Task3", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task3", IsDirty = true });
            var mockPopupController = new Mock<IPopupController>();
            mockPopupController.Setup(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.Yes).Verifiable();
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(false).Verifiable();

            var schedulerViewModel = new SchedulerViewModelForTest(env.Object, new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopupController.Object, new SynchronousAsyncWorker());

            schedulerViewModel.CurrentEnvironment = env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);

            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            auth.Verify(a => a.IsAuthorized(AuthorizationContext.Administrator, null));
            Assert.AreEqual(schedulerViewModel.Errors.FetchErrors().Count, 1);
            Assert.AreEqual(@"Error while saving: You don't have permission to schedule on this server. You need Administrator permission.", schedulerViewModel.Errors.FetchErrors().FirstOrDefault());
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_WithNewNameDiffToOldNameNoDialogResponse_DialogShowsConflict()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResource("Task2", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task1", IsDirty = true };
            scheduledResourceForTest.IsDirty = true;
            resources.Add(scheduledResourceForTest);
            resources.Add(new ScheduledResource("Task3", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task3", IsDirty = true });
            var mockPopupController = new Mock<IPopupController>();
            mockPopupController.Setup(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.No).Verifiable();
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();

            var schedulerViewModel = new SchedulerViewModelForTest(env.Object, new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopupController.Object, new SynchronousAsyncWorker());
            schedulerViewModel.CurrentEnvironment = env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            mockScheduledResourceModel.Setup(model => model.Save(It.IsAny<IScheduledResource>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            var _nameChanged = false;


            schedulerViewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case "Name":
                        _nameChanged = true;
                        break;
                    default:
                        break;
                }
            };

            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsTrue(_nameChanged);
            Assert.IsTrue(schedulerViewModel.GetCredentialsCalled);
            string errorMessage;
            mockScheduledResourceModel.Verify(model => model.Save(It.IsAny<IScheduledResource>(), out errorMessage), Times.Once());
            Assert.AreEqual("Task1", schedulerViewModel.TaskList[0].Name);
            mockPopupController.Verify(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_WithNewNameDiffToOldNameCancelDialogResponse_DialogShowsConflict()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResource("Task2", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task1", IsDirty = true };
            scheduledResourceForTest.IsDirty = true;
            resources.Add(scheduledResourceForTest);
            resources.Add(new ScheduledResource("Task3", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task3", IsDirty = true });
            var mockPopupController = new Mock<IPopupController>();
            mockPopupController.Setup(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.Cancel).Verifiable();
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();

            var schedulerViewModel = new SchedulerViewModelForTest(env.Object, new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopupController.Object, new SynchronousAsyncWorker());
            schedulerViewModel.CurrentEnvironment = env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            mockScheduledResourceModel.Setup(model => model.Save(It.IsAny<IScheduledResource>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsFalse(schedulerViewModel.GetCredentialsCalled);
            string errorMessage;
            mockScheduledResourceModel.Verify(model => model.Save(It.IsAny<IScheduledResource>(), out errorMessage), Times.Never());
            Assert.AreEqual("Task2", schedulerViewModel.TaskList[0].Name);
            mockPopupController.Verify(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_WithNewNameDiffToOldNameCancelDialogResponse_IsNewTrue_ShouldNotShowDialog()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResource("Task2", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task1", IsDirty = true };
            scheduledResourceForTest.IsDirty = true;
            resources.Add(scheduledResourceForTest);
            resources.Add(new ScheduledResource("Task3", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task3", IsDirty = true });
            var mockPopupController = new Mock<IPopupController>();
            mockPopupController.Setup(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.Cancel).Verifiable();
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();

            var schedulerViewModel = new SchedulerViewModelForTest(env.Object, new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopupController.Object, new SynchronousAsyncWorker());
            schedulerViewModel.CurrentEnvironment = env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            mockScheduledResourceModel.Setup(model => model.Save(It.IsAny<IScheduledResource>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            schedulerViewModel.SelectedTask.IsNew = true;
            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            mockPopupController.Verify(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_WithNewNameDiffToOldNameCancelDialogResponse_IsNewFalse_ShouldShowDialog()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResource("Task2", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task1", IsDirty = true };
            scheduledResourceForTest.IsDirty = true;
            resources.Add(scheduledResourceForTest);
            resources.Add(new ScheduledResource("Task3", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1", Guid.NewGuid().ToString()) { OldName = "Task3", IsDirty = true });
            var mockPopupController = new Mock<IPopupController>();
            mockPopupController.Setup(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.Cancel).Verifiable();
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();

            var schedulerViewModel = new SchedulerViewModelForTest(env.Object, new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopupController.Object, new SynchronousAsyncWorker());
            schedulerViewModel.CurrentEnvironment = env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            mockScheduledResourceModel.Setup(model => model.Save(It.IsAny<IScheduledResource>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            schedulerViewModel.SelectedTask.IsNew = false;
            schedulerViewModel.SelectedTask.IsDirty = true;
            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsFalse(schedulerViewModel.GetCredentialsCalled);
            string errorMessage;
            mockScheduledResourceModel.Verify(model => model.Save(It.IsAny<IScheduledResource>(), out errorMessage), Times.Never());
            Assert.AreEqual("Task2", schedulerViewModel.TaskList[0].Name);
            mockPopupController.Verify(c => c.ShowNameChangedConflict(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_UserNamePasswordSet_CallsScheduledResourceModelSave()
        {
            //------------Setup for test--------------------------
            var scheduledResourceForTest = new ScheduledResourceForTest { IsDirty = true };
            var resources = new ObservableCollection<IScheduledResource>();
            resources.Add(scheduledResourceForTest);
            scheduledResourceForTest.UserName = "some user";
            scheduledResourceForTest.Password = "some password";

            var env = new Mock<IServer>();
            var schedulerViewModel = new SchedulerViewModelForTest(env.Object);
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();
            schedulerViewModel.CurrentEnvironment = env.Object;
            schedulerViewModel.ToEnvironmentModel = a => env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            mockScheduledResourceModel.Setup(model => model.Save(It.IsAny<IScheduledResource>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsTrue(schedulerViewModel.ShowSaveErrorDialogCalled);
            Assert.IsFalse(schedulerViewModel.GetCredentialsCalled);
            string errorMessage;
            mockScheduledResourceModel.Verify(model => model.Save(It.IsAny<IScheduledResource>(), out errorMessage), Times.Once());
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_SaveCommand")]
        public void SchedulerViewModel_SaveCommand_UserNamePasswordSet_CallsIsAuthorised()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResourceForTest { IsDirty = true };
            resources.Add(scheduledResourceForTest);
            scheduledResourceForTest.UserName = "some user";
            scheduledResourceForTest.Password = "some password";
            var schedulerViewModel = new SchedulerViewModelForTest(new Mock<IServer>().Object);
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();
            schedulerViewModel.CurrentEnvironment = env.Object;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            mockScheduledResourceModel.Setup(model => model.Save(It.IsAny<IScheduledResource>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];
            //------------Execute Test---------------------------
            schedulerViewModel.SaveCommand.Execute(null);
            //------------Assert Results-------------------------
            Assert.IsTrue(schedulerViewModel.ShowSaveErrorDialogCalled);
            Assert.IsFalse(schedulerViewModel.GetCredentialsCalled);
            string errorMessage;
            mockScheduledResourceModel.Verify(model => model.Save(It.IsAny<IScheduledResource>(), out errorMessage), Times.Once());
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_SelectedTask")]
        public void SchedulerViewModel_SelectedTask_SetValue_ShouldFirePropertyChangedNotifications()
        {
            //------------Setup for test--------------------------
            var _selectedTaskChanged = false;
            var _triggerChange = false;
            var _statusChange = false;
            var _workflowNameChange = false;
            var _nameChange = false;
            var _runasapChange = false;
            var _numRecordsChange = false;
            var _triggerTextChange = false;
            var _historyMustChange = false;
            var _accountNameChange = false;
            var _passwordChange = false;
            var _errorsChange = false;
            var _errorChange = false;
            var _selectedHistoryChange = false;

            var agg = new Mock<IEventAggregator>();
            agg.Setup(a => a.Publish(It.IsAny<DebugOutputMessage>())).Verifiable();
            var schedulerViewModel = new SchedulerViewModel(agg.Object, new DirectoryObjectPickerDialog(), new PopupController(), new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);


            schedulerViewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case "SelectedTask":
                        _selectedTaskChanged = true;
                        break;
                    case "Trigger":
                        _triggerChange = true;
                        break;
                    case "Status":
                        _statusChange = true;
                        break;
                    case "WorkflowName":
                        _workflowNameChange = true;
                        break;
                    case "Name":
                        _nameChange = true;
                        break;
                    case "RunAsapIfScheduleMissed":
                        _runasapChange = true;
                        break;
                    case "NumberOfRecordsToKeep":
                        _numRecordsChange = true;
                        break;
                    case "TriggerText":
                        _triggerTextChange = true;
                        break;
                    case "History":
                        _historyMustChange = true;
                        break;
                    case "AccountName":
                        _accountNameChange = true;
                        break;
                    case "Password":
                        _passwordChange = true;
                        break;
                    case "Errors":
                        _errorsChange = true;
                        break;
                    case "Error":
                        _errorChange = true;
                        break;
                    case "SelectedHistory":
                        _selectedHistoryChange = true;
                        break;
                    default:
                        break;
                }
            };
            //------------Execute Test---------------------------
            schedulerViewModel.SelectedTask = new ScheduledResourceForTest();
            //------------Assert Results-------------------------
            Assert.IsTrue(_selectedTaskChanged);
            Assert.IsTrue(_triggerChange);
            Assert.IsTrue(_statusChange);
            Assert.IsTrue(_workflowNameChange);
            Assert.IsTrue(_nameChange);
            Assert.IsTrue(_runasapChange);
            Assert.IsTrue(_numRecordsChange);
            Assert.IsTrue(_triggerTextChange);
            Assert.IsTrue(_historyMustChange);
            Assert.IsTrue(_accountNameChange);
            Assert.IsTrue(_passwordChange);
            Assert.IsTrue(_errorsChange);
            Assert.IsTrue(_errorChange);
            Assert.IsTrue(_selectedHistoryChange);
            agg.Verify(a => a.Publish(It.IsAny<DebugOutputMessage>()));
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_SelectedTask")]
        public void SchedulerViewModel_SelectedTask_SetValueWithHistoryTab_ShouldFirePropertyChangedNotifications()
        {
            //------------Setup for test--------------------------
            var _selectedTaskChanged = false;
            var _triggerChange = false;
            var _statusChange = false;
            var _workflowNameChange = false;
            var _nameChange = false;
            var _runasapChange = false;
            var _numRecordsChange = false;
            var _triggerTextChange = false;
            var _historyMustChange = false;
            var _accountNameChange = false;
            var _passwordChange = false;
            var _errorsChange = false;
            var _errorChange = false;
            var _selectedHistoryChange = false;
            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object);
            var activeItem = new TabItem { Header = "History" };
            schedulerViewModel.ActiveItem = activeItem;
            schedulerViewModel.ActiveItem = activeItem;
            schedulerViewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case "SelectedTask":
                        _selectedTaskChanged = true;
                        break;
                    case "Trigger":
                        _triggerChange = true;
                        break;
                    case "Status":
                        _statusChange = true;
                        break;
                    case "WorkflowName":
                        _workflowNameChange = true;
                        break;
                    case "Name":
                        _nameChange = true;
                        break;
                    case "RunAsapIfScheduleMissed":
                        _runasapChange = true;
                        break;
                    case "NumberOfRecordsToKeep":
                        _numRecordsChange = true;
                        break;
                    case "TriggerText":
                        _triggerTextChange = true;
                        break;
                    case "History":
                        _historyMustChange = true;
                        break;
                    case "AccountName":
                        _accountNameChange = true;
                        break;
                    case "Password":
                        _passwordChange = true;
                        break;
                    case "Errors":
                        _errorsChange = true;
                        break;
                    case "Error":
                        _errorChange = true;
                        break;
                    case "SelectedHistory":
                        _selectedHistoryChange = true;
                        break;
                    default:
                        break;
                }
            };
            //------------Execute Test---------------------------
            schedulerViewModel.SelectedTask = new ScheduledResourceForTest();
            //------------Assert Results-------------------------
            Assert.IsTrue(_selectedTaskChanged);
            Assert.IsTrue(_triggerChange);
            Assert.IsTrue(_statusChange);
            Assert.IsTrue(_workflowNameChange);
            Assert.IsTrue(_nameChange);
            Assert.IsTrue(_runasapChange);
            Assert.IsTrue(_numRecordsChange);
            Assert.IsTrue(_triggerTextChange);
            Assert.IsTrue(_historyMustChange);
            Assert.IsTrue(_accountNameChange);
            Assert.IsTrue(_passwordChange);
            Assert.IsTrue(_errorsChange);
            Assert.IsTrue(_errorChange);
            Assert.IsTrue(_selectedHistoryChange);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_Name")]
        public void SchedulerViewModel_Name_EmptyString_AddsErrorMessage()
        {
            //------------Setup for test--------------------------

            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);

            var scheduledResourceForTest = new ScheduledResourceForTest { Name = "Test" };
            schedulerViewModel.SelectedTask = scheduledResourceForTest;

            //------------------Assert Preconditions---------------------------
            Assert.AreNotEqual(null, schedulerViewModel.SelectedTask, "Scheduler view model selected task cannot be set.");

            //------------Execute Test---------------------------
            schedulerViewModel.Name = "";

            //------------Assert Results-------------------------
            Assert.IsTrue(schedulerViewModel.HasErrors, "Scheduler view model does not have errors after name is made empty.");
            Assert.AreEqual("The name can not be blank", schedulerViewModel.Error);
            Assert.AreEqual(string.Empty, schedulerViewModel.SelectedTask.Name);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_Name")]
        public void SchedulerViewModel_Name_WasEmptyStringValidString_ClearsErrorMessage()
        {
            //------------Setup for test--------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);

            var scheduledResourceForTest = new ScheduledResourceForTest { Name = "Test" };
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            schedulerViewModel.Name = "";

            //------------------Assert Preconditions---------------------------
            Assert.AreNotEqual(null, schedulerViewModel.SelectedTask, "Scheduler view model selected task cannot be set.");
            Assert.AreEqual(string.Empty, schedulerViewModel.Name, "Scheduler view model name cannot be set.");
            Assert.IsTrue(schedulerViewModel.HasErrors, "Scheduler view model does not have errors when its name is empty.");
            Assert.AreEqual("The name can not be blank", schedulerViewModel.Error);

            //------------Execute Test---------------------------
            schedulerViewModel.Name = "This is a test";

            //------------Assert Results-------------------------
            Assert.IsFalse(schedulerViewModel.HasErrors);
            Assert.AreEqual(string.Empty, schedulerViewModel.Error);
            Assert.AreEqual("This is a test", schedulerViewModel.SelectedTask.Name);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_WorkflowName")]
        public void SchedulerViewModel_WorkflowName_BlankName_SetsError()
        {
            //------------Setup for test--------------------------

            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);

            var scheduledResourceForTest = new ScheduledResourceForTest { WorkflowName = "Test" };
            schedulerViewModel.SelectedTask = scheduledResourceForTest;

            //------------Execute Test---------------------------
            schedulerViewModel.WorkflowName = "";
            //------------Assert Results-------------------------
            Assert.IsTrue(schedulerViewModel.HasErrors);
            Assert.AreEqual("Please select a workflow to schedule", schedulerViewModel.Error);
            Assert.AreEqual(string.Empty, schedulerViewModel.SelectedTask.WorkflowName);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_Name")]
        public void SchedulerViewModel_Name_DuplicateName_SetsError()
        {
            //------------Setup for test--------------------------

            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResourceForTest { Name = "Test" };
            var scheduledResourceForTest2 = new ScheduledResourceForTest { Name = "Test2" };
            scheduledResourceForTest.IsDirty = true;
            resources.Add(scheduledResourceForTest);
            resources.Add(scheduledResourceForTest2);
            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object);

            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            //------------Execute Test---------------------------
            schedulerViewModel.Name = "Test2";
            //------------Assert Results-------------------------
            Assert.IsTrue(schedulerViewModel.HasErrors);
            Assert.AreEqual("There is already a task with the same name", schedulerViewModel.Error);
            Assert.AreEqual("Test2", schedulerViewModel.SelectedTask.Name);
            Assert.IsTrue(schedulerViewModel.SelectedTask.IsDirty);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_Name")]
        public void SchedulerViewModel_Name_NonDuplicateName_ClearsError()
        {
            //------------Setup for test--------------------------

            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResourceForTest { Name = "Test" };
            var scheduledResourceForTest2 = new ScheduledResourceForTest { Name = "Test2" };
            scheduledResourceForTest.IsDirty = true;
            resources.Add(scheduledResourceForTest);
            resources.Add(scheduledResourceForTest2);
            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object);

            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            schedulerViewModel.Name = "Test2";
            //------------Assert Preconditions------------------
            Assert.IsTrue(schedulerViewModel.HasErrors);
            Assert.AreEqual("There is already a task with the same name", schedulerViewModel.Error);
            Assert.AreEqual("Test2", schedulerViewModel.SelectedTask.Name);
            Assert.IsTrue(schedulerViewModel.SelectedTask.IsDirty);

            //------------Execute Test---------------------------
            schedulerViewModel.Name = "Test Some";
            //------------Assert Results-------------------------
            Assert.IsFalse(schedulerViewModel.HasErrors);
            Assert.AreEqual(string.Empty, schedulerViewModel.Error);
            Assert.AreEqual("Test Some", schedulerViewModel.SelectedTask.Name);
            Assert.IsTrue(schedulerViewModel.SelectedTask.IsDirty);

        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_NumberOfRecordsToKeep")]
        public void SchedulerViewModel_NumberOfRecordsToKeep_NotWholeNumber_KeepsOldNumber()
        {
            //------------Setup for test--------------------------

            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResourceForTest { Name = "Test" };
            var scheduledResourceForTest2 = new ScheduledResourceForTest { Name = "Test2" };
            scheduledResourceForTest.IsDirty = false;
            resources.Add(scheduledResourceForTest);
            resources.Add(scheduledResourceForTest2);
            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object);

            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            mockScheduledResourceModel.Setup(model => model.ScheduledResources).Returns(resources);
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = scheduledResourceForTest;
            //------------Execute Test---------------------------
            schedulerViewModel.NumberOfRecordsToKeep = "5";
            schedulerViewModel.NumberOfRecordsToKeep = "-a5";
            //------------Assert Results-------------------------
            Assert.AreEqual(5, schedulerViewModel.SelectedTask.NumberOfHistoryToKeep);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_History")]
        public void SchedulerViewModel_History_Get_ShouldCallCreateHistoryOnScheduledResourceModel()
        {
            //------------Setup for test--------------------------
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var activeItem = new TabItem { Header = "History" };
            schedulerViewModel.ActiveItem = activeItem;
            var mockScheduledResourceModel = new Mock<IScheduledResourceModel>();
            var histories = new List<IResourceHistory> { new Mock<IResourceHistory>().Object };
            mockScheduledResourceModel.Setup(model => model.CreateHistory(It.IsAny<IScheduledResource>())).Returns(histories);
            schedulerViewModel.ScheduledResourceModel = mockScheduledResourceModel.Object;
            schedulerViewModel.SelectedTask = new Mock<IScheduledResource>().Object;
            //------------Execute Test---------------------------
            var resourceHistories = schedulerViewModel.History;
            //------------Assert Results-------------------------
            mockScheduledResourceModel.Verify(model => model.CreateHistory(It.IsAny<IScheduledResource>()), Times.Once());
            Assert.IsNotNull(resourceHistories);
            Assert.IsTrue(schedulerViewModel.IsHistoryTabVisible);
            Assert.IsFalse(schedulerViewModel.IsProgressBarVisible);

            schedulerViewModel.IsProgressBarVisible = false;
            Assert.IsFalse(schedulerViewModel.IsProgressBarVisible);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_ActiveItem")]
        public void SchedulerViewModel_ActiveItem_HeaderNotHistory_ShouldNotFirePropertyChangeOnHistory()
        {
            //------------Setup for test--------------------------
            var _historyMustChange = false;
            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object);
            var activeItem = new TabItem { Header = "Settings" };
            schedulerViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "History")
                {
                    _historyMustChange = true;
                }
            };
            //------------Execute Test---------------------------
            schedulerViewModel.ActiveItem = activeItem;
            //------------Assert Results-------------------------
            Assert.IsFalse(_historyMustChange);
        }




        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_ActiveItem")]
        public void SchedulerViewModel_ActiveItem_HeaderHistory_ShouldFirePropertyChangeOnHistory()
        {
            //------------Setup for test--------------------------
            var server = new Mock<IServer>();
            server.Setup(a => a.DisplayName).Returns("LocalHost");
            var shell = new Mock<IShellViewModel>();
            CustomContainer.Register<IShellViewModel>(shell.Object);
            shell.Setup(a => a.LocalhostServer).Returns(server.Object);
            shell.Setup(a => a.ActiveServer).Returns(server.Object);
            var _historyMustChange = false;
            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object);
            var activeItem = new TabItem { Header = "History" };
            schedulerViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "History")
                {
                    _historyMustChange = true;
                }
            };
            //------------Execute Test---------------------------
            schedulerViewModel.ActiveItem = activeItem;
            //------------Assert Results-------------------------
            Assert.IsTrue(_historyMustChange);
        }

        [TestMethod]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("SchedulerViewModel_Validation")]
        public void SchedulerViewModel_Validation_NoErrorsWhenNothingSelected()
        {
            var resourceModel = new Mock<IScheduledResourceModel>();
            var resources = new ObservableCollection<IScheduledResource> { new ScheduledResource("bob", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()), new ScheduledResource("dave", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) };
            resourceModel.Setup(a => a.ScheduledResources).Returns(resources);

            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object)
            {
                ScheduledResourceModel = resourceModel.Object
            };
            Assert.IsFalse(schedulerViewModel.HasErrors);

        }
        [TestMethod]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("SchedulerViewModel_Validation")]
        public void SchedulerViewModel_Validation_NoErrorOnSelected()
        {
            var resourceModel = new Mock<IScheduledResourceModel>();
            var resources = new ObservableCollection<IScheduledResource> { new ScheduledResource("bob", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()), new ScheduledResource("dave", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) };
            resourceModel.Setup(a => a.ScheduledResources).Returns(resources);

            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object)
            {
                ScheduledResourceModel = resourceModel.Object,
                SelectedTask = resources[0],
                Name = "monkeys"
            };
            // validation occurs on property changes
            Assert.IsFalse(schedulerViewModel.HasErrors);

        }

        [TestMethod]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("SchedulerViewModel_Validation")]
        public void SchedulerViewModel_Validation_IfDuplicateNames()
        {
            var resourceModel = new Mock<IScheduledResourceModel>();
            var resources = new ObservableCollection<IScheduledResource> { new ScheduledResource("bob", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) { NumberOfHistoryToKeep = 1 }, new ScheduledResource("dave", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) };
            resourceModel.Setup(a => a.ScheduledResources).Returns(resources);

            var schedulerViewModel = new SchedulerViewModel(a => new Mock<IServer>().Object)
            {
                ScheduledResourceModel = resourceModel.Object,
                SelectedTask = resources[0],
                Name = "dave"
            };
            //create duplicate name
            Assert.IsTrue(schedulerViewModel.HasErrors);
            Assert.AreEqual("There is already a task with the same name", schedulerViewModel.Error);
            Assert.IsTrue(resources.All(a => a.Errors.HasErrors()));
            Assert.IsTrue(resources.All(a => a.Errors.FetchErrors()[0] == "There is already a task with the same name"));


        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_DeleteTask")]
        public void SchedulerViewModel_DeleteTask_DeleteSecondTask_ShouldDeleteTaskFromList()
        {
            //------------Setup for test--------------------------

            var mockPopUpController = new Mock<IPopupController>();
            mockPopUpController.Setup(c => c.ShowDeleteConfirmation(It.IsAny<string>())).Returns(MessageBoxResult.Yes);
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopUpController.Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();
            schedulerViewModel.CurrentEnvironment = env.Object;
            var resources = new ObservableCollection<IScheduledResource> { new ScheduledResource("bob", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) { NumberOfHistoryToKeep = 1 }, new ScheduledResource("dave", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) };

            var resourceModel = new Mock<IScheduledResourceModel>();
            resourceModel.Setup(c => c.ScheduledResources).Returns(resources);
            resourceModel.Setup(c => c.DeleteSchedule(It.IsAny<IScheduledResource>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = resourceModel.Object;
            //------------Execute Test---------------------------
            Assert.AreEqual(2, schedulerViewModel.TaskList.Count);
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[1];

            var _taskListChange = false;
            var _historyChange = false;

            schedulerViewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case "TaskList":
                        _taskListChange = true;
                        break;
                    case "History":
                        _historyChange = true;
                        break;
                    default:
                        break;
                }
            };

            schedulerViewModel.DeleteCommand.Execute(resources[1]);
            //------------Assert Results-------------------------
            Assert.IsTrue(_taskListChange);
            Assert.IsTrue(_historyChange);
            Assert.AreEqual(1, schedulerViewModel.TaskList.Count);
            Assert.AreEqual(schedulerViewModel.SelectedTask, schedulerViewModel.TaskList[0]);
            Assert.AreEqual("bob", schedulerViewModel.TaskList[0].Name);
            Assert.AreEqual(SchedulerStatus.Enabled, schedulerViewModel.TaskList[0].Status);
            resourceModel.Verify(a => a.DeleteSchedule(It.IsAny<IScheduledResource>()), Times.Once());
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_DeleteTask")]
        public void SchedulerViewModel_DeleteTask_DeleteFirstTask_ShouldDeleteTaskFromList()
        {
            //------------Setup for test--------------------------

            var mockPopUpController = new Mock<IPopupController>();
            mockPopUpController.Setup(c => c.ShowDeleteConfirmation(It.IsAny<string>())).Returns(MessageBoxResult.Yes);
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopUpController.Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();
            schedulerViewModel.CurrentEnvironment = env.Object;
            var resources = new ObservableCollection<IScheduledResource> { new ScheduledResource("bob", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) { NumberOfHistoryToKeep = 1 }, new ScheduledResource("dave", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) };

            var resourceModel = new Mock<IScheduledResourceModel>();
            resourceModel.Setup(c => c.ScheduledResources).Returns(resources);
            resourceModel.Setup(c => c.DeleteSchedule(It.IsAny<IScheduledResource>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = resourceModel.Object;
            //------------Execute Test---------------------------
            Assert.AreEqual(2, schedulerViewModel.TaskList.Count);
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];

            schedulerViewModel.DeleteCommand.Execute(resources[0]);
            //------------Assert Results-------------------------
            Assert.AreEqual(1, schedulerViewModel.TaskList.Count);
            Assert.AreEqual(schedulerViewModel.SelectedTask, schedulerViewModel.TaskList[0]);
            Assert.AreEqual("dave", schedulerViewModel.TaskList[0].Name);
            Assert.AreEqual(SchedulerStatus.Enabled, schedulerViewModel.TaskList[0].Status);
            resourceModel.Verify(a => a.DeleteSchedule(It.IsAny<IScheduledResource>()), Times.Once());
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_DeleteTask")]
        public void SchedulerViewModel_DeleteTask_DeleteWithNoAdminRights_ShouldShowError()
        {
            //------------Setup for test--------------------------

            var mockPopUpController = new Mock<IPopupController>();
            mockPopUpController.Setup(c => c.ShowDeleteConfirmation(It.IsAny<string>())).Returns(MessageBoxResult.Yes);
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopUpController.Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(true);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(false).Verifiable();
            schedulerViewModel.CurrentEnvironment = env.Object;
            var resources = new ObservableCollection<IScheduledResource> { new ScheduledResource("bob", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) { NumberOfHistoryToKeep = 1 }, new ScheduledResource("dave", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) };

            var resourceModel = new Mock<IScheduledResourceModel>();
            resourceModel.Setup(c => c.ScheduledResources).Returns(resources);
            resourceModel.Setup(c => c.DeleteSchedule(It.IsAny<IScheduledResource>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = resourceModel.Object;
            //------------Execute Test---------------------------
            Assert.AreEqual(2, schedulerViewModel.TaskList.Count);
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];

            schedulerViewModel.DeleteCommand.Execute(resources[0]);
            //------------Assert Results-------------------------
            Assert.AreEqual(@"Error while saving: You don't have permission to schedule on this server. You need Administrator permission.", schedulerViewModel.Error);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_DeleteTask")]
        public void SchedulerViewModel_DeleteTask_DeleteWhenEnvironmentIsntConnected_ShouldShowError()
        {
            //------------Setup for test--------------------------

            var mockPopUpController = new Mock<IPopupController>();
            mockPopUpController.Setup(c => c.ShowDeleteConfirmation(It.IsAny<string>())).Returns(MessageBoxResult.Yes);
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopUpController.Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object);
            var env = new Mock<IServer>();
            var auth = new Mock<IAuthorizationService>();
            env.Setup(a => a.IsConnected).Returns(false);
            env.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(true).Verifiable();
            schedulerViewModel.CurrentEnvironment = env.Object;
            var resources = new ObservableCollection<IScheduledResource> { new ScheduledResource("bob", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) { NumberOfHistoryToKeep = 1 }, new ScheduledResource("dave", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "c", Guid.NewGuid().ToString()) };

            var resourceModel = new Mock<IScheduledResourceModel>();
            resourceModel.Setup(c => c.ScheduledResources).Returns(resources);
            resourceModel.Setup(c => c.DeleteSchedule(It.IsAny<IScheduledResource>())).Verifiable();
            schedulerViewModel.ScheduledResourceModel = resourceModel.Object;
            //------------Execute Test---------------------------
            Assert.AreEqual(2, schedulerViewModel.TaskList.Count);
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];

            schedulerViewModel.DeleteCommand.Execute(resources[0]);
            //------------Assert Results-------------------------
            Assert.AreEqual("Error while saving: Server unreachable.", schedulerViewModel.Error);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("SchedulerViewModel_EditTrigger")]
        public void SchedulerViewModel_EditTrigger_ShouldEditTheTrigger()
        {
            //------------Setup for test--------------------------
            CustomContainer.Register(new Mock<IServerRepository>().Object);
            var schedulerViewModel = new SchedulerViewModelForTest(new Mock<IServer>().Object);
            var resources = new ObservableCollection<IScheduledResource>();
            var mockScheduleTrigger = new Mock<IScheduleTrigger>();
            mockScheduleTrigger.Setup(c => c.Trigger.Instance).Returns(new DailyTrigger());
            resources.Add(new ScheduledResource("bob", SchedulerStatus.Enabled, DateTime.MaxValue, mockScheduleTrigger.Object, "c", Guid.NewGuid().ToString()) { NumberOfHistoryToKeep = 1 });
            resources.Add(new ScheduledResource("dave", SchedulerStatus.Enabled, DateTime.MaxValue, mockScheduleTrigger.Object, "c", Guid.NewGuid().ToString()));

            var resourceModel = new Mock<IScheduledResourceModel>();
            resourceModel.Setup(c => c.ScheduledResources).Returns(resources);
            schedulerViewModel.ScheduledResourceModel = resourceModel.Object;
            //------------Execute Test---------------------------
            Assert.AreEqual(2, schedulerViewModel.TaskList.Count);
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];

            var _triggerTextChange = false;

            schedulerViewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case "TriggerText":
                        _triggerTextChange = true;
                        break;
                    default:
                        break;
                }
            };

            schedulerViewModel.EditTriggerCommand.Execute(null);
            //------------Assert Results-------------------------

            Assert.IsNotNull(schedulerViewModel.SchedulerTaskManager.TriggerEditDialog);
            Assert.IsTrue(_triggerTextChange);
            Assert.AreEqual("bob", schedulerViewModel.SelectedTask.Name);
            Assert.AreEqual(2013, schedulerViewModel.SelectedTask.NextRunDate.Year);
            Assert.AreEqual(02, schedulerViewModel.SelectedTask.NextRunDate.Hour);
            Assert.AreEqual(21, schedulerViewModel.SelectedTask.NextRunDate.Minute);
            Assert.IsTrue(schedulerViewModel.TriggerText.StartsWith("At"));
            Assert.IsTrue(schedulerViewModel.TriggerText.EndsWith("AM every day"), $"TriggerText \"{schedulerViewModel.TriggerText}\" does not end with \"AM every day\"");

            schedulerViewModel.SelectedTask = null;
            Assert.AreEqual("", schedulerViewModel.TriggerText);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("SchedulerViewModel_AddWorkflow")]
        public void SchedulerViewModel_AddWorkflow_WithNewTaskNameSet_WorkflowNameChangedAndNameChanged()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource> { new ScheduledResource("New Task1", SchedulerStatus.Enabled, DateTime.MaxValue, new Mock<IScheduleTrigger>().Object, "TestFlow1",Guid.NewGuid().ToString()
                ) { NumberOfHistoryToKeep = 1 } };
            var mockEnvironmentModel = new Mock<IServer>();
            var mockConnection = new Mock<IEnvironmentConnection>();
            mockConnection.Setup(connection => connection.IsConnected).Returns(true);
            mockConnection.Setup(connection => connection.WorkspaceID).Returns(Guid.NewGuid());
            mockEnvironmentModel.Setup(model => model.Connection).Returns(mockConnection.Object);
            mockEnvironmentModel.Setup(model => model.IsConnected).Returns(true);
            var resourceRepo = new ResourceRepository(mockEnvironmentModel.Object);
            var setupResourceModelMock = Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow2");
            var resId = Guid.NewGuid();
            setupResourceModelMock.Setup(c => c.ID).Returns(resId);
            resourceRepo.Add(Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow1").Object);
            resourceRepo.Add(setupResourceModelMock.Object);
            resourceRepo.Add(Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow3").Object);
            mockEnvironmentModel.Setup(c => c.ResourceRepository).Returns(resourceRepo);

            var mockResourcePickerDialog = new Mock<IResourcePickerDialog>();
            mockResourcePickerDialog.Setup(c => c.ShowDialog(It.IsAny<IServer>())).Returns(true);

            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, new Mock<IPopupController>().Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object) { CurrentEnvironment = mockEnvironmentModel.Object };

            var treeItem = new Mock<IExplorerTreeItem>();
            treeItem.Setup(a => a.ResourceName).Returns("TestFlow2");
            treeItem.Setup(a => a.ResourceId).Returns(resId);
            treeItem.Setup(a => a.ResourcePath).Returns("Category\\Testing");
            mockResourcePickerDialog.Setup(c => c.SelectedResource).Returns(treeItem.Object);

            schedulerViewModel.SchedulerTaskManager.CurrentResourcePickerDialog = mockResourcePickerDialog.Object;

            var scheduledResourceModelMock = new Mock<IScheduledResourceModel>();
            scheduledResourceModelMock.Setup(c => c.ScheduledResources).Returns(resources);
            schedulerViewModel.ScheduledResourceModel = scheduledResourceModelMock.Object;
            //------------Execute Test---------------------------
            schedulerViewModel.TaskList[0].WorkflowName = "TestFlow3";
            schedulerViewModel.SelectedTask = schedulerViewModel.TaskList[0];

            var _nameChange = false;
            var _workflowNameChange = false;
            var _taskListChange = false;



            schedulerViewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case "TaskList":
                        _taskListChange = true;
                        break;
                    case "Name":
                        _nameChange = true;
                        break;
                    case "WorkflowName":
                        _workflowNameChange = true;
                        break;
                    default:
                        break;
                }
            };

            var canExec = schedulerViewModel.AddWorkflowCommand.CanExecute(null);
            Assert.IsTrue(canExec);

            schedulerViewModel.AddWorkflowCommand.Execute(null);
            //------------Assert Results-------------------------  
            Assert.IsTrue(_nameChange);
            Assert.IsTrue(_workflowNameChange);
            Assert.IsTrue(_taskListChange);
            Assert.IsTrue(schedulerViewModel.SelectedTask.IsDirty);
            Assert.AreEqual("TestFlow2", schedulerViewModel.Name);
            Assert.AreEqual("Category\\Testing", schedulerViewModel.WorkflowName);
            Assert.AreEqual(resId, schedulerViewModel.SelectedTask.ResourceId);
        }

        [TestMethod]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("SchedulerViewModel_DeActivate")]
        public void SchedulerViewModel_DeActivateDiscard_ReturnsTrue()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResourceForTest { Trigger = new ScheduleTrigger(TaskState.Ready, new Dev2DailyTrigger(new TaskServiceConvertorFactory(), new DailyTrigger()), new Dev2TaskService(new TaskServiceConvertorFactory()), new TaskServiceConvertorFactory()), IsDirty = true };
            resources.Add(scheduledResourceForTest);
            var serializer = new Dev2JsonSerializer();
            var serializeObject = serializer.SerializeToBuilder(resources);
            var mockEnvironmentModel = new Mock<IServer>();
            var mockConnection = new Mock<IEnvironmentConnection>();
            mockConnection.Setup(connection => connection.IsConnected).Returns(true);
            mockConnection.Setup(connection => connection.ExecuteCommand(It.IsAny<StringBuilder>(), It.IsAny<Guid>())).Returns(serializeObject);
            mockConnection.Setup(connection => connection.WorkspaceID).Returns(Guid.NewGuid());
            mockEnvironmentModel.Setup(model => model.Connection).Returns(mockConnection.Object);
            mockEnvironmentModel.Setup(model => model.IsConnected).Returns(true);
            var resourceRepo = new ResourceRepository(mockEnvironmentModel.Object);
            var setupResourceModelMock = Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow2");
            resourceRepo.Add(Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow1").Object);
            resourceRepo.Add(setupResourceModelMock.Object);
            resourceRepo.Add(Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow3").Object);
            mockEnvironmentModel.Setup(c => c.ResourceRepository).Returns(resourceRepo);
            var popup = new Mock<IPopupController>();
            popup.Setup(a => a.ShowSchedulerCloseConfirmation()).Returns(MessageBoxResult.No);
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, popup.Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object) { SelectedTask = scheduledResourceForTest, CurrentEnvironment = null };
            //------------Execute Test---------------------------
            Assert.IsTrue(schedulerViewModel.DoDeactivate(true));
            //------------Assert Results-------------------------

        }

        [TestMethod]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("SchedulerViewModel_TriggerEquals")]
        public void SchedulerViewModel_TriggerEquals_Equals_ExpectTrue()
        {
            //------------Setup for test--------------------------


            var t = new DailyTrigger();

            t.StartBoundary = new DateTime(2014, 01, 01);
            var t2 = new DailyTrigger();
            t2.StartBoundary = new DateTime(2014, 01, 01);

            //------------Execute Test---------------------------

            //------------Assert Results-------------------------
            Assert.IsTrue(SchedulerTaskManager.TriggerEquals(t, t2));
        }


        [TestMethod]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("SchedulerViewModel_TriggerEquals")]
        public void SchedulerViewModel_TriggerEquals_NotEquals_ExpectFalse()
        {
            //------------Setup for test--------------------------


            var t = new DailyTrigger();

            t.StartBoundary = new DateTime(2014, 01, 01);
            var t2 = new DailyTrigger();
            t2.StartBoundary = new DateTime(2015, 01, 01);

            //------------Execute Test---------------------------

            //------------Assert Results-------------------------
            Assert.IsFalse(SchedulerTaskManager.TriggerEquals(t, t2));
        }

        [TestMethod]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("SchedulerViewModel_DeActivate")]
        public void SchedulerViewModel_DeActivateCancel_ReturnsFalse()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResourceForTest { Trigger = new ScheduleTrigger(TaskState.Ready, new Dev2DailyTrigger(new TaskServiceConvertorFactory(), new DailyTrigger()), new Dev2TaskService(new TaskServiceConvertorFactory()), new TaskServiceConvertorFactory()), IsDirty = true };
            resources.Add(scheduledResourceForTest);
            var serializer = new Dev2JsonSerializer();
            var serializeObject = serializer.SerializeToBuilder(resources);
            var mockEnvironmentModel = new Mock<IServer>();
            var mockConnection = new Mock<IEnvironmentConnection>();
            mockConnection.Setup(connection => connection.IsConnected).Returns(true);
            mockConnection.Setup(connection => connection.ExecuteCommand(It.IsAny<StringBuilder>(), It.IsAny<Guid>())).Returns(serializeObject);
            mockConnection.Setup(connection => connection.WorkspaceID).Returns(Guid.NewGuid());
            mockEnvironmentModel.Setup(model => model.Connection).Returns(mockConnection.Object);
            mockEnvironmentModel.Setup(model => model.IsConnected).Returns(true);
            var resourceRepo = new ResourceRepository(mockEnvironmentModel.Object);
            var setupResourceModelMock = Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow2");
            resourceRepo.Add(Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow1").Object);
            resourceRepo.Add(setupResourceModelMock.Object);
            resourceRepo.Add(Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow3").Object);
            mockEnvironmentModel.Setup(c => c.ResourceRepository).Returns(resourceRepo);
            var popup = new Mock<IPopupController>();
            popup.Setup(a => a.ShowSchedulerCloseConfirmation()).Returns(MessageBoxResult.Cancel);
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, popup.Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object) { SelectedTask = scheduledResourceForTest, CurrentEnvironment = null };
            //------------Execute Test---------------------------
            Assert.IsFalse(schedulerViewModel.DoDeactivate(true));
            //------------Assert Results-------------------------

        }


        [TestMethod]
        [Owner("Leon Rajindrapersadh")]
        [TestCategory("SchedulerViewModel_HandleServerSelectionChangedMessage")]
        public void SchedulerViewModel_DeActivateSave_AttemptsSave()
        {
            //------------Setup for test--------------------------
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResourceForTest { Trigger = new ScheduleTrigger(TaskState.Ready, new Dev2DailyTrigger(new TaskServiceConvertorFactory(), new DailyTrigger()), new Dev2TaskService(new TaskServiceConvertorFactory()), new TaskServiceConvertorFactory()), IsDirty = true };
            resources.Add(scheduledResourceForTest);
            var serializer = new Dev2JsonSerializer();
            var serializeObject = serializer.SerializeToBuilder(resources);
            var mockEnvironmentModel = new Mock<IServer>();
            var mockConnection = new Mock<IEnvironmentConnection>();
            mockConnection.Setup(connection => connection.IsConnected).Returns(true);
            mockConnection.Setup(connection => connection.ExecuteCommand(It.IsAny<StringBuilder>(), It.IsAny<Guid>())).Returns(serializeObject);
            mockConnection.Setup(connection => connection.WorkspaceID).Returns(Guid.NewGuid());
            mockEnvironmentModel.Setup(model => model.Connection).Returns(mockConnection.Object);
            mockEnvironmentModel.Setup(model => model.IsConnected).Returns(true);
            var resourceRepo = new ResourceRepository(mockEnvironmentModel.Object);
            var setupResourceModelMock = Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow2");
            resourceRepo.Add(Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow1").Object);
            resourceRepo.Add(setupResourceModelMock.Object);
            resourceRepo.Add(Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow3").Object);
            mockEnvironmentModel.Setup(c => c.ResourceRepository).Returns(resourceRepo);
            var popup = new Mock<IPopupController>();
            popup.Setup(a => a.ShowSchedulerCloseConfirmation()).Returns(MessageBoxResult.Yes);
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, popup.Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object) { SelectedTask = scheduledResourceForTest };

            var auth = new Mock<IAuthorizationService>();
            mockEnvironmentModel.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(false).Verifiable();
            schedulerViewModel.CurrentEnvironment = mockEnvironmentModel.Object;
            //------------Execute Test---------------------------
            schedulerViewModel.DoDeactivate(true);
            //------------Assert Results-------------------------
            auth.Verify(a => a.IsAuthorized(AuthorizationContext.Administrator, null));
            Assert.AreEqual(@"Error while saving: You don't have permission to schedule on this server. You need Administrator permission.", schedulerViewModel.Error);

            schedulerViewModel.SelectedTask = null;
            schedulerViewModel.DoDeactivate(true);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SchedulerViewModel_HandleServerSelectionChangedMessage")]
        public void SchedulerViewModel_FalseDeActivateSave_AttemptsSave()
        {
            //------------Setup for test--------------------------
            var serverRepo = new Mock<IServerRepository>();
            CustomContainer.Register(serverRepo.Object);
            var resources = new ObservableCollection<IScheduledResource>();
            var scheduledResourceForTest = new ScheduledResourceForTest { Trigger = new ScheduleTrigger(TaskState.Ready, new Dev2DailyTrigger(new TaskServiceConvertorFactory(), new DailyTrigger()), new Dev2TaskService(new TaskServiceConvertorFactory()), new TaskServiceConvertorFactory()), IsDirty = true };
            resources.Add(scheduledResourceForTest);
            var serializer = new Dev2JsonSerializer();
            var serializeObject = serializer.SerializeToBuilder(resources);
            var mockEnvironmentModel = new Mock<IServer>();
            var mockConnection = new Mock<IEnvironmentConnection>();
            mockConnection.Setup(connection => connection.IsConnected).Returns(true);
            mockConnection.Setup(connection => connection.ExecuteCommand(It.IsAny<StringBuilder>(), It.IsAny<Guid>())).Returns(serializeObject);
            mockConnection.Setup(connection => connection.WorkspaceID).Returns(Guid.NewGuid());
            mockEnvironmentModel.Setup(model => model.Connection).Returns(mockConnection.Object);
            mockEnvironmentModel.Setup(model => model.IsConnected).Returns(true);
            var resourceRepo = new ResourceRepository(mockEnvironmentModel.Object);
            var setupResourceModelMock = Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow2");
            resourceRepo.Add(Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow1").Object);
            resourceRepo.Add(setupResourceModelMock.Object);
            resourceRepo.Add(Dev2MockFactory.SetupResourceModelMock(ResourceType.WorkflowService, "TestFlow3").Object);
            mockEnvironmentModel.Setup(c => c.ResourceRepository).Returns(resourceRepo);
            var popup = new Mock<IPopupController>();
            popup.Setup(a => a.ShowSchedulerCloseConfirmation()).Returns(MessageBoxResult.Yes);
            var schedulerViewModel = new SchedulerViewModel(new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, popup.Object, new SynchronousAsyncWorker(), new Mock<IServer>().Object, a => new Mock<IServer>().Object) { SelectedTask = scheduledResourceForTest };

            var auth = new Mock<IAuthorizationService>();
            mockEnvironmentModel.Setup(a => a.AuthorizationService).Returns(auth.Object);
            auth.Setup(a => a.IsAuthorized(AuthorizationContext.Administrator, null)).Returns(false).Verifiable();
            schedulerViewModel.CurrentEnvironment = mockEnvironmentModel.Object;
            //------------Execute Test---------------------------
            schedulerViewModel.DoDeactivate(false);
            //------------Assert Results-------------------------
            auth.Verify(a => a.IsAuthorized(AuthorizationContext.Administrator, null));
            Assert.AreEqual(@"Error while saving: You don't have permission to schedule on this server. You need Administrator permission.", schedulerViewModel.Error);

            schedulerViewModel.SelectedTask = null;
            schedulerViewModel.DoDeactivate(false);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SchedulerViewModel_ShowSaveErrorDialog")]
        public void SchedulerViewModel_ShowSaveErrorDialog_GivenMessage_Result()
        {
            //------------Setup for test--------------------------
            var errorMessageUsed = "This is an error";
            string errorMessageCalled = null;
            var mockPopupController = new Mock<IPopupController>();
            mockPopupController.Setup(controller => controller.ShowSaveErrorDialog(It.IsAny<string>())).Callback(
                (string errorMessage) =>
                {
                    errorMessageCalled = errorMessage;
                });

            var env = new Mock<IServer>();

            var schedulerViewModel = new SchedulerViewModelForTest(env.Object, new Mock<IEventAggregator>().Object, new Mock<DirectoryObjectPickerDialog>().Object, mockPopupController.Object, new SynchronousAsyncWorker());
            //------------Execute Test---------------------------
            schedulerViewModel.CallShowErrorDialog(errorMessageUsed);
            //------------Assert Results-------------------------
            Assert.IsNotNull(errorMessageCalled);
            Assert.AreEqual(errorMessageUsed, errorMessageCalled);
        }
    }


}
