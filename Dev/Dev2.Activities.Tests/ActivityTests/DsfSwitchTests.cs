﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dev2.Activities;
using Dev2.Common.Interfaces.Diagnostics.Debug;
using Dev2.Common.State;
using Dev2.Communication;
using Dev2.Data.SystemTemplates.Models;
using Dev2.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Unlimited.Applications.BusinessDesignStudio.Activities;
using Warewolf.Storage;
using Warewolf.Storage.Interfaces;

namespace Dev2.Tests.Activities.ActivityTests
{
    class DsfSwitchMock : DsfSwitch
    {
        public DsfSwitchMock(DsfFlowSwitchActivity activity)
            : base(activity)
        {

        }

        public void ExecuteMock(IDSFDataObject dataObject, int update)
        {
            ExecuteTool(dataObject, update);
        }
    }
    [TestClass]
    public class DsfSwitchTests
    {
        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void Constructor_GivenIsNew_ShouldHaveCorrectValues()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString()
            };
            var activity = new DsfSwitch(switchActivity);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity);
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.AreEqual("Switch", activity.DisplayName);
            Assert.AreSame(switchActivity, activity.Inner);
            Assert.AreSame(switchActivity.UniqueID, activity.UniqueID);
            Assert.IsNull(activity.Switches);
            Assert.IsNull(activity.Default);
            Assert.IsNull(activity.Switch);
            Assert.IsNull(activity.Result);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void GetOutputs_GivenIsNew_ShouldZeroOutputs()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString()
            };
            var activity = new DsfSwitch(switchActivity);
            //---------------Assert Precondition----------------
            Assert.AreEqual("Switch", activity.DisplayName);
            Assert.AreSame(switchActivity, activity.Inner);
            Assert.AreSame(switchActivity.UniqueID, activity.UniqueID);
            Assert.IsNull(activity.Switches);
            Assert.IsNull(activity.Default);
            Assert.IsNull(activity.Switch);
            Assert.IsNull(activity.Result);
            //---------------Execute Test ----------------------
            var outputs = activity.GetOutputs();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, outputs.Count);

        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void GetForEachInputs_ReturnsNull()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString()
            };
            var activity = new DsfSwitch(switchActivity);
            //---------------Assert Precondition----------------
            var outputs = activity.GetOutputs();
            Assert.AreEqual(0, outputs.Count);
            //---------------Execute Test ----------------------
            var dsfForEachItems = activity.GetForEachInputs();
            //---------------Test Result -----------------------
            Assert.IsNull(dsfForEachItems);

        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void GetForEachOutputs_ReturnsNull()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString()
            };
            var activity = new DsfSwitch(switchActivity);
            //---------------Assert Precondition----------------
            var outputs = activity.GetForEachInputs();
            Assert.IsNull(outputs);
            //---------------Execute Test ----------------------
            var dsfForEachItems = activity.GetForEachOutputs();
            //---------------Test Result -----------------------
            Assert.IsNull(dsfForEachItems);

        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void GetDebugInputs_GivenIsNewReturnsZero()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString()
            };
            var activity = new DsfSwitch(switchActivity);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var customAttributes = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, customAttributes.Count);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void GetDebugOutputs_GivenIsNewReturnsZero()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString()
            };
            var activity = new DsfSwitch(switchActivity);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var customAttributes = activity.GetDebugOutputs(new Mock<IExecutionEnvironment>().Object, 1);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, customAttributes.Count);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void DebugOutput_GivenDataObject_ShouldSetInnerBugOuputsIncrementsDebugOutputs()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString()
            };
            var activity = new DsfSwitch(switchActivity)
            {
                Result = "[[MyResult]]"
            };
            var dataObject = new Mock<IDSFDataObject>();
            dataObject.Setup(o => o.IsDebugMode()).Returns(true);
            var obj = new Warewolf.Testing.PrivateObject(activity);
            //---------------Assert Precondition----------------
            var activityDebugOutputs = activity.GetDebugOutputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(0, activityDebugOutputs.Count);
            //---------------Execute Test ----------------------
            obj.Invoke("DebugOutput", dataObject.Object);
            //---------------Test Result -----------------------
            activityDebugOutputs = activity.GetDebugOutputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(1, activityDebugOutputs.Count);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void Debug_GivenDataObject_ShouldSetInnerBugOuputs_IncrementsDebugInputs()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString(),
                ExpressionText = ""
            };

            var activity = new DsfSwitch(switchActivity)
            {
                Result = "[[MyResult]]"
            };
            var dataObject = new Mock<IDSFDataObject>();
            dataObject.Setup(o => o.IsDebugMode()).Returns(true);
            var obj = new Warewolf.Testing.PrivateObject(activity);
            //---------------Assert Precondition----------------
            var getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(0, getDebugInputs.Count);
            //---------------Execute Test ----------------------
            var result = "[[variable]]";
            var mySwitch = new Dev2Switch();
            obj.Invoke("Debug", dataObject.Object, result, mySwitch);
            //---------------Test Result -----------------------
            getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(1, getDebugInputs.Count);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void ExecuteTool_GivenIsNotDebugMode_NotAddDebugOutputs()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString(),
                ExpressionText = ""
            };
            var nextActivity = new Mock<IDev2Activity>();
            var activity = new DsfSwitchMock(switchActivity)
            {
                Result = "[[MyResult]]",
                Switch = "[[Switch]]",
                Switches = new Dictionary<string, IDev2Activity>()
            };
            activity.Switches.Add("1", nextActivity.Object);
            var dataObject = new Mock<IDSFDataObject>();
            dataObject.Setup(o => o.IsDebugMode()).Returns(false);
            var executionEnvironment = new ExecutionEnvironment();
            executionEnvironment.Assign("[[Switch]]", "1", 1);
            dataObject.Setup(o => o.Environment).Returns(executionEnvironment);
            //---------------Assert Precondition----------------
            var getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(0, getDebugInputs.Count);
            //---------------Execute Test ----------------------
            activity.ExecuteMock(dataObject.Object, 0);
            //---------------Test Result -----------------------
            getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(0, getDebugInputs.Count);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void ExecuteTool_GivenSwicthMacthing_ShouldAddNextNodes()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString(),
                ExpressionText = ""
            };
            var nextActivity = new Mock<IDev2Activity>();
            var activity = new DsfSwitchMock(switchActivity)
            {
                Result = "[[MyResult]]",
                Switch = "[[Switch]]",
                Switches = new Dictionary<string, IDev2Activity>()
            };
            activity.Switches.Add("1", nextActivity.Object);
            var dataObject = new Mock<IDSFDataObject>();
            dataObject.Setup(o => o.IsDebugMode()).Returns(false);
            var executionEnvironment = new ExecutionEnvironment();
            executionEnvironment.Assign("[[Switch]]", "1", 1);
            dataObject.Setup(o => o.Environment).Returns(executionEnvironment);
            //---------------Assert Precondition----------------
            var getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(0, getDebugInputs.Count);
            Assert.IsNull(activity.NextNodes);
            //---------------Execute Test ----------------------
            activity.ExecuteMock(dataObject.Object, 0);
            //---------------Test Result -----------------------
            getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(0, getDebugInputs.Count);
            Assert.AreEqual(1, activity.NextNodes.Count());
            var contains = activity.NextNodes.Contains(nextActivity.Object);
            Assert.IsTrue(contains);
        }
        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void ExecuteTool_GivenSwicthNotMacthing_ShouldUseDefault()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString(),
                ExpressionText = ""
            };
            var nextActivity = new Mock<IDev2Activity>();
            var activity = new DsfSwitchMock(switchActivity)
            {
                Result = "[[MyResult]]",
                Switch = "[[Switch]]",
                Switches = new Dictionary<string, IDev2Activity>(),
                Default = new List<IDev2Activity> { nextActivity.Object }
            };
            var dataObject = new Mock<IDSFDataObject>();
            dataObject.Setup(o => o.IsDebugMode()).Returns(false);
            var executionEnvironment = new ExecutionEnvironment();
            executionEnvironment.Assign("[[Switch]]", "NoMatch", 1);
            dataObject.Setup(o => o.Environment).Returns(executionEnvironment);
            //---------------Assert Precondition----------------
            var getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(0, getDebugInputs.Count);
            Assert.IsNull(activity.NextNodes);
            //---------------Execute Test ----------------------
            activity.ExecuteMock(dataObject.Object, 0);
            //---------------Test Result -----------------------
            getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(0, getDebugInputs.Count);
            Assert.IsNotNull(activity.NextNodes);
            Assert.AreEqual(1, activity.NextNodes.Count());
            var contains = activity.NextNodes.Single();
            Assert.AreEqual(nextActivity.Object, contains);
            Assert.AreEqual("Default", activity.Result);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Nkosinathi Sangweni")]
        public void ExecuteTool_GivenIsDebugMode_ShouldHaveDebugOutputs()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString(),
                ExpressionText = ""
            };
            var nextActivity = new Mock<IDev2Activity>();
            var activity = new DsfSwitchMock(switchActivity)
            {
                Result = "[[MyResult]]",
                Switch = "[[Switch]]",
                Switches = new Dictionary<string, IDev2Activity>(),
                Default = new List<IDev2Activity> { nextActivity.Object }
            };
            var dataObject = new Mock<IDSFDataObject>();
            dataObject.Setup(o => o.IsDebugMode()).Returns(true);
            var executionEnvironment = new ExecutionEnvironment();
            executionEnvironment.Assign("[[Switch]]", "NoMatch", 1);
            dataObject.Setup(o => o.Environment).Returns(executionEnvironment);
            //---------------Assert Precondition----------------
            var getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(0, getDebugInputs.Count);
            Assert.IsNull(activity.NextNodes);
            //---------------Execute Test ----------------------
            activity.ExecuteMock(dataObject.Object, 0);
            //---------------Test Result -----------------------
            getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(1, getDebugInputs.Count);
            Assert.IsNotNull(activity.NextNodes);
            Assert.AreEqual(1, activity.NextNodes.Count());
            var contains = activity.NextNodes.Single();
            Assert.AreEqual(nextActivity.Object, contains);
            Assert.AreEqual("Default", activity.Result);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Pieter Terblanche")]
        public void ExecuteTool_GivenDefaultIsNull_ShouldShowError()
        {
            //---------------Set up test pack-------------------
            //string displayName, IDebugDispatcher debugDispatcher, bool isAsync = false
            var switchActivity = new DsfFlowSwitchActivity("MyName", new Mock<IDebugDispatcher>().Object, It.IsAny<bool>())
            {
                UniqueID = Guid.NewGuid().ToString(),
                ExpressionText = ""
            };
            var nextActivity = new Mock<IDev2Activity>();
            var activity = new DsfSwitchMock(switchActivity)
            {
                Result = "[[MyResult]]",
                Switch = "[[Switch]]",
                Switches = new Dictionary<string, IDev2Activity>(),
                Default = null
            };
            var dataObject = new Mock<IDSFDataObject>();
            dataObject.Setup(o => o.IsDebugMode()).Returns(true);
            var executionEnvironment = new ExecutionEnvironment();
            executionEnvironment.Assign("[[Switch]]", "NoMatch", 1);
            dataObject.Setup(o => o.Environment).Returns(executionEnvironment);
            //---------------Assert Precondition----------------
            var getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(0, getDebugInputs.Count);
            Assert.IsNull(activity.NextNodes);
            //---------------Execute Test ----------------------
            activity.ExecuteMock(dataObject.Object, 0);
            //---------------Test Result -----------------------
            getDebugInputs = activity.GetDebugInputs(new Mock<IExecutionEnvironment>().Object, 1);
            Assert.AreEqual(1, getDebugInputs.Count);
            Assert.IsNotNull(activity.NextNodes);
            Assert.AreEqual(0, activity.NextNodes.Count());
            Assert.AreEqual(1, executionEnvironment.AllErrors.Count);
            Assert.AreEqual("Failed: Switch must have a Default arm connected.",executionEnvironment.AllErrors.First());
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Pieter Terblanche")]
        [TestCategory("DsfSwitch_GetState")]
        public void DsfSwitch_GetState_ReturnsStateVariable()
        {
            //---------------Set up test pack-------------------
            var nextActivity = new Mock<IDev2Activity>();
            var serializer = new Dev2JsonSerializer();
            //------------Setup for test--------------------------

            const string expectedResult = "[[MyResult]]";
            const string expectedSwitch = "[[Switch]]";
            var expectedSwitches = new Dictionary<string, IDev2Activity>();
            var expectedDefault = new List<IDev2Activity> { nextActivity.Object };

            var act = new DsfSwitch
            {
                Result = expectedResult,
                Switch = expectedSwitch,
                Switches = expectedSwitches,
                Default = expectedDefault
            };
            //------------Execute Test---------------------------
            var stateItems = act.GetState();
            Assert.AreEqual(4, stateItems.Count());

            var expectedResults = new[]
            {
                new StateVariable
                {
                    Name = "Switch",
                    Type = StateVariable.StateType.Input,
                    Value = expectedSwitch
                },
                new StateVariable
                {
                    Name = "Switches",
                    Type = StateVariable.StateType.Output,
                    Value = serializer.Serialize(expectedSwitches)
                },
                 new StateVariable
                {
                    Name = "Default",
                    Type = StateVariable.StateType.Output,
                    Value = serializer.Serialize(expectedDefault)
                },
                new StateVariable
                {
                    Name = "Result",
                    Value = expectedResult,
                    Type = StateVariable.StateType.Output
                }
            };

            var iter = act.GetState().Select(
                (item, index) => new
                {
                    value = item,
                    expectValue = expectedResults[index]
                }
                );

            //------------Assert Results-------------------------
            foreach (var entry in iter)
            {
                Assert.AreEqual(entry.expectValue.Name, entry.value.Name);
                Assert.AreEqual(entry.expectValue.Type, entry.value.Type);
                Assert.AreEqual(entry.expectValue.Value, entry.value.Value);
            }
        }
    }
}