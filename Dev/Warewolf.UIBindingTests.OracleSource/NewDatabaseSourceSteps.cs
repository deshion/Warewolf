﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.SaveDialog;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Studio.Core;
using Dev2.Studio.Interfaces;
using Dev2.Threading;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.Studio.Core.Infragistics_Prism_Region_Adapter;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;
using Warewolf.UIBindingTests.Core;



namespace Warewolf.UIBindingTests.OracleSource
{
    [Binding]
    public class NewDatabaseSourceSteps
    {
        string loginFailedForUserTest = "Login failed for user 'test'";
        static FeatureContext _featureContext;
        static ScenarioContext _scenarioContext;

        public NewDatabaseSourceSteps(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

        [BeforeFeature("DbSource")]
        public static void SetupForSystem(FeatureContext featureContext)
        {
            _featureContext = featureContext;
            Utils.SetupResourceDictionary();
            var manageDatabaseSourceControl = new ManageDatabaseSourceControl();
            var mockStudioUpdateManager = new Mock<IManageDatabaseSourceModel>();
            mockStudioUpdateManager.Setup(model => model.GetComputerNames()).Returns(new List<string> { "TEST", "RSAKLFSVRDEV", "RSAKLFSVRPDC", "TFSBLD.premier.local", "RSAKLFSVRWRWBLD" });
            mockStudioUpdateManager.Setup(model => model.ServerName).Returns("localhost");
            var mockRequestServiceNameViewModel = new Mock<IRequestServiceNameViewModel>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockExecutor = new Mock<IExternalProcessExecutor>();
            var task = new Task<IRequestServiceNameViewModel>(() => mockRequestServiceNameViewModel.Object);
            task.Start();
            var manageDatabaseSourceViewModel = new ManageOracleSourceViewModel(mockStudioUpdateManager.Object, task, mockEventAggregator.Object, new SynchronousAsyncWorker());
            manageDatabaseSourceControl.DataContext = manageDatabaseSourceViewModel;
            Utils.ShowTheViewForTesting(manageDatabaseSourceControl);
            _featureContext.Add(Utils.ViewNameKey, manageDatabaseSourceControl);
            _featureContext.Add(Utils.ViewModelNameKey, manageDatabaseSourceViewModel);
            _featureContext.Add("updateManager", mockStudioUpdateManager);
            _featureContext.Add("requestServiceNameViewModel", mockRequestServiceNameViewModel);
            _featureContext.Add("externalProcessExecutor", mockExecutor);
        }

        [BeforeScenario("OracleDbSource")]
        public void SetupForDatabaseSource()
        {
            _scenarioContext.Add(Utils.ViewNameKey, _featureContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey));
            _scenarioContext.Add("updateManager", _featureContext.Get<Mock<IManageDatabaseSourceModel>>("updateManager"));
            _scenarioContext.Add("requestServiceNameViewModel", _featureContext.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel"));
            _scenarioContext.Add(Utils.ViewModelNameKey, _featureContext.Get<ManageOracleSourceViewModel>(Utils.ViewModelNameKey));
        }

        [Then(@"""(.*)"" tab is opened")]
        public void ThenTabIsOpened(string headerText)
        {
            var viewModel = _scenarioContext.Get<IDockAware>("viewModel");
            Assert.AreEqual(headerText, viewModel.Header);
        }

        [Then(@"title is ""(.*)""")]
        public void ThenTitleIs(string p0)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            Assert.AreEqual(manageDatabaseSourceControl.GetHeader(), p0);
        }

        [Then(@"""(.*)"" is the tab Header")]
        public void ThenIsTheTabHeader(string p0)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            Assert.AreEqual(manageDatabaseSourceControl.GetTabHeader(), p0);
        }

        [Given(@"I open New Database Source")]
        public void GivenIOpenNewDatabaseSource()
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            Assert.IsNotNull(manageDatabaseSourceControl);
        }

        [Given(@"I open ""(.*)""")]
        [When(@"I open ""(.*)""")]
        public void GivenIOpen(string name)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            var mock = _featureContext.Get<Mock<IManageDatabaseSourceModel>>("updateManager");
            var upd = mock.Object;
            var dbsrc = new DbSourceDefinition
            {
                Name = name,
                Id = Guid.NewGuid(),
                ServerName = "RSAKLFSVRDEV",
                AuthenticationType = AuthenticationType.Windows
            };
            mock.Setup(model => model.FetchDbSource(It.IsAny<Guid>())).Returns(dbsrc);
            _featureContext["dbsrc"] = dbsrc;
            var mockEventAggregator = new Mock<IEventAggregator>();
            var viewModel = new ManageOracleSourceViewModel(upd, mockEventAggregator.Object, dbsrc, new SynchronousAsyncWorker());
            if (manageDatabaseSourceControl.DataContext is ManageOracleSourceViewModel manageDatabaseSourceViewModel)
            {
                Utils.ResetViewModel<ManageOracleSourceViewModel, IDbSource>(viewModel, manageDatabaseSourceViewModel);
            }
        }

        [Given(@"Server as ""(.*)""")]
        [When(@"Server as ""(.*)""")]
        public void GivenServerAs(string server)
        {
            var db = _featureContext.Get<IDbSource>("dbsrc");
            db.ServerName = server;
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.SelectServer("server");
        }

        [When(@"I Edit Server as ""(.*)""")]
        public void WhenIEditServerAs(string server)
        {
            var db = _featureContext.Get<IDbSource>("dbsrc");
            db.ServerName = server;
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.SelectServer("server");
        }

        [Then(@"Authentication Type is selected as ""(.*)""")]
        public void ThenAuthenticationTypeIsSelectedAs(string authstr)
        {
            var db = _featureContext.Get<IDbSource>("dbsrc");
            var authp = Enum.Parse(typeof(AuthenticationType), authstr);
            db.AuthenticationType = (AuthenticationType)authp;

            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);

            if (manageDatabaseSourceControl.DataContext is ManageOracleSourceViewModel manageDatabaseSourceViewModel)
            {
                Assert.AreEqual(manageDatabaseSourceViewModel.AuthenticationType, (AuthenticationType)authp);
            }
        }

        [Given(@"Username field is ""(.*)""")]
        public void GivenUsernameFieldIs(string user)
        {
            var db = _featureContext.Get<IDbSource>("dbsrc");
            db.UserName = user;
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.EnterUserName(user);
        }

        [Given(@"Password field is ""(.*)""")]
        public void GivenPasswordFieldIs(string pwd)
        {
            var db = _featureContext.Get<IDbSource>("dbsrc");
            db.Password = pwd;
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.EnterPassword(pwd);
        }

        [Given(@"Database ""(.*)"" is selected")]
        public void GivenDatabaseIsSelected(string dbName)
        {
            var db = _featureContext.Get<IDbSource>("dbsrc");
            db.DbName = dbName;
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.SelectDatabase(dbName);
        }

        [Then(@"Database ""(.*)"" is selected")]
        public void ThenDatabaseIsSelected(string dbName)
        {
            var db = _featureContext.Get<IDbSource>("dbsrc");
            db.DbName = dbName;
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.SelectDatabase(dbName);
        }

        [When(@"I type Server as ""(.*)""")]
        public void WhenITypeServerAs(string p0)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.SelectServer(p0);
        }

        [Then(@"I type Select The Server as ""(.*)""")]
        public void ThenITypeSelectTheServerAs(string p0)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.SelectServer(p0);
        }

        [Then(@"the intellisense contains these options")]
        public void ThenTheIntellisenseContainsTheseOptions(Table table)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);

            var rows = table.Rows[0].Values;
            foreach (var server in rows)
            {
                manageDatabaseSourceControl.VerifyServerExistsintComboBox(server);
            }
        }

        [Given(@"I type Server as ""(.*)""")]
        public void GivenITypeServerAs(string serverName)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.SelectServer(serverName);
            //manageDatabaseSourceControl.EnterServerName(serverName);
            var viewModel = _scenarioContext.Get<ManageOracleSourceViewModel>("viewModel");
            Assert.AreEqual(serverName, viewModel.ServerName.Name);
        }

        [Given(@"Database dropdown is ""(.*)""")]
        [When(@"Database dropdown is ""(.*)""")]
        [Then(@"Database dropdown is ""(.*)""")]
        public void GivenDropdownIs(string visibility)
        {
            var expectedVisibility = String.Equals(visibility, "Collapsed", StringComparison.InvariantCultureIgnoreCase) ? Visibility.Collapsed : Visibility.Visible;

            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            var databaseDropDownVisibility = manageDatabaseSourceControl.GetDatabaseDropDownVisibility();
            Assert.AreEqual(expectedVisibility, databaseDropDownVisibility);
        }

        [Given(@"""(.*)"" is ""(.*)""")]
        [When(@"""(.*)"" is ""(.*)""")]
        [Then(@"""(.*)"" is ""(.*)""")]
        public void GivenIs(string controlName, string enabledString)
        {
            Utils.CheckControlEnabled(controlName, enabledString, _scenarioContext.Get<ICheckControlEnabledView>(Utils.ViewNameKey), Utils.ViewNameKey);
        }

        [Given(@"I select ""(.*)"" as Database")]
        [When(@"I select ""(.*)"" as Database")]
        [Then(@"I select ""(.*)"" as Database")]
        public void WhenISelectAsDatabase(string databaseName)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.SelectDatabase(databaseName);
            var viewModel = (ManageOracleSourceViewModel)manageDatabaseSourceControl.DataContext;
            Assert.AreEqual(databaseName, viewModel.DatabaseName);
        }

        [When(@"I save the source")]
        public void WhenISaveTheSource()
        {
            var mockRequestServiceNameViewModel = _scenarioContext.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Verifiable();
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.PerformSave();
        }

        [When(@"I save the source as ""(.*)""")]
        public void WhenISaveTheSourceAs(string name)
        {
            var mockRequestServiceNameViewModel = _scenarioContext.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Returns(MessageBoxResult.OK).Verifiable();
            mockRequestServiceNameViewModel.Setup(a => a.ResourceName).Returns(new ResourceName("", name));
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.PerformSave();
        }



        [Then(@"Username is ""(.*)""")]
        public void ThenUsernameIs(string userName)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);

            Assert.AreEqual(userName, manageDatabaseSourceControl.GetUsername());
        }

        [Then(@"Password  is ""(.*)""")]
        public void ThenPasswordIs(string password)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);

            Assert.AreEqual(password, manageDatabaseSourceControl.GetPassword());
        }



        [Given(@"I type Username as ""(.*)""")]
        [When(@"I type Username as ""(.*)""")]
        [Then(@"I type Username as ""(.*)""")]
        public void WhenITypeUsernameAs(string userName)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.EnterUserName(userName);
            var viewModel = _scenarioContext.Get<ManageOracleSourceViewModel>("viewModel");
            Assert.AreEqual(userName, viewModel.UserName);
        }

        [Given(@"I type Password as ""(.*)""")]
        [When(@"I type Password as ""(.*)""")]
        [Then(@"I type Password as ""(.*)""")]
        public void WhenITypePasswordAs(string password)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.EnterPassword(password);
            var viewModel = _scenarioContext.Get<ManageOracleSourceViewModel>("viewModel");
            Assert.AreEqual(password, viewModel.Password);
        }

        [Then(@"the error message is ""(.*)""")]
        public void ThenTheErrorMessageIs(string errorMessage)
        {
            errorMessage = "Exception: " + loginFailedForUserTest + Environment.NewLine + Environment.NewLine +
                           "Inner Exception: " + loginFailedForUserTest;
            var viewModel = _scenarioContext.Get<ManageOracleSourceViewModel>("viewModel");
            Debug.WriteLine(errorMessage);
            Debug.WriteLine(viewModel.TestMessage);
            const string Error = "Inner Exception: Login failed for user 'test'";
            var contains = viewModel.TestMessage.Contains(Error);
            Assert.IsTrue(contains);
        }


        [Then(@"Test Connecton is ""(.*)""")]
        [When(@"Test Connecton is ""(.*)""")]
        public void ThenTestConnectonIs(string successString)
        {
            var mockUpdateManager = _scenarioContext.Get<Mock<IManageDatabaseSourceModel>>("updateManager");
            var isSuccess = String.Equals(successString, "Successful", StringComparison.InvariantCultureIgnoreCase);
            var isLongRunning = String.Equals(successString, "Long Running", StringComparison.InvariantCultureIgnoreCase);
            if (isSuccess)
            {
                mockUpdateManager.Setup(manager => manager.TestDbConnection(It.IsAny<IDbSource>()))
                    .Returns(new List<string> { "Dev2TestingDB" });
            }
            else if (isLongRunning)
            {
                var viewModel = _scenarioContext.Get<ManageOracleSourceViewModel>("viewModel");
                mockUpdateManager.Setup(manager => manager.TestDbConnection(It.IsAny<IDbSource>()));
                viewModel.AsyncWorker = new AsyncWorker();
            }
            else
            {
                mockUpdateManager.Setup(manager => manager.TestDbConnection(It.IsAny<IDbSource>()))
                    .Throws(new WarewolfTestException(loginFailedForUserTest, new Exception(loginFailedForUserTest)));
            }
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.PerformTestConnection();
            Thread.Sleep(1000);
        }

        [When(@"I Cancel the Test")]
        public void WhenICancelTheTest()
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.CancelTest();
        }

        [Then(@"the validation message as ""(.*)""")]
        public void ThenTheValidationMessageAs(string message)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            var viewModel = _scenarioContext.Get<ManageOracleSourceViewModel>("viewModel");
            var errorMessageFromControl = manageDatabaseSourceControl.GetErrorMessage();
            var errorMessageOnViewModel = viewModel.TestMessage;
            var isErrorMessageOnControl = errorMessageFromControl.Equals(message, StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(isErrorMessageOnControl);
            var isErrorMessage = errorMessageOnViewModel.Equals(message, StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(isErrorMessage);
        }

        [Then(@"the save dialog is opened")]
        public void ThenTheSaveDialogIsOpened()
        {
            var mockRequestServiceNameViewModel = _scenarioContext.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Verify();
        }

        [AfterFeature("OracleDbSource")]
        public static void FeaureCleanup()
        {
            CleanupResources();
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            Utils.CloseViewAfterTesting(manageDatabaseSourceControl);
        }

        static void CleanupResources()
        {
            var mockUpdateManager = _scenarioContext.Get<Mock<IManageDatabaseSourceModel>>("updateManager");
            var mockRequestServiceNameViewModel =
                _scenarioContext.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            var mockEventAggregator = new Mock<IEventAggregator>();
            var task = new Task<IRequestServiceNameViewModel>(() => mockRequestServiceNameViewModel.Object);
            task.Start();
            var viewModel = new ManageOracleSourceViewModel(mockUpdateManager.Object, task, mockEventAggregator.Object,
                new SynchronousAsyncWorker());
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            if (manageDatabaseSourceControl.DataContext is ManageOracleSourceViewModel manageDatabaseSourceViewModel)
            {
                Utils.ResetViewModel<ManageOracleSourceViewModel, IDbSource>(viewModel, manageDatabaseSourceViewModel);
                manageDatabaseSourceViewModel.DatabaseName = null;
            }

        }

        [AfterScenario("OracleDbSource")]
        public void Cleanup()
        {
            CleanupResources();
        }

        [When(@"I click ""(.*)""")]
        public void WhenIClick(string ConectTo)
        {
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            manageDatabaseSourceControl.Test();
        }



        [Then(@"Authentication type ""(.*)"" is ""(.*)""")]
        public void ThenAuthenticationTypeIs(string p0, string p1)
        {
            Utils.CheckControlEnabled(p0, p1, _scenarioContext.Get<ICheckControlEnabledView>(Utils.ViewNameKey), Utils.ViewNameKey);
        }


        [Then(@"database dropdown is ""(.*)""")]
        public void ThenDatabaseDropdownIs(string p0)
        {
            var expectedVisibility = String.Equals(p0, "Collapsed", StringComparison.InvariantCultureIgnoreCase) ? Visibility.Collapsed : Visibility.Visible;
            var manageDatabaseSourceControl = _scenarioContext.Get<ManageDatabaseSourceControl>(Utils.ViewNameKey);
            var databaseDropDownVisibility = manageDatabaseSourceControl.GetDatabaseDropDownVisibility();
            Assert.AreEqual(expectedVisibility, databaseDropDownVisibility);
        }
    }
}
