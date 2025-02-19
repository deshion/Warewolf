﻿using Dev2.Common.State;
using Dev2.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Unlimited.Applications.BusinessDesignStudio.Activities;

namespace Dev2.Tests.Activities.ActivityComparerTests.NumberFormat
{
    [TestClass]
    public class DsfFindRecordsMultipleCriteriaActivityComparerTest
    {
        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_DifferentUniqueIds_ActivityTools_AreNotEqual()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId };
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity);
            //---------------Execute Test ----------------------
            var @equals = activity.Equals(activity1);
            //---------------Test Result -----------------------
            Assert.IsTrue(@equals);
        }
        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_EmptyActivityTools_AreEqual()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId };
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity);
            //---------------Execute Test ----------------------
            var @equals = activity.Equals(activity1);
            //---------------Test Result -----------------------
            Assert.IsTrue(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_DisplayName_Same_DisplayName_IsEqual()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, DisplayName = "a" };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, DisplayName = "a" };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsTrue(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_DisplayName_Different_DisplayName_Is_Not_Equal()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, DisplayName = "A" };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, DisplayName = "ass" };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsFalse(@equals);
        }
        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Result_Same_Result_IsEqual()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, Result = "a" };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, Result = "a" };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsTrue(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Result_Different_Result_Is_Not_Equal()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, Result = "A" };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, Result = "ass" };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsFalse(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Same_FieldsToSearch_IsEqual()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, FieldsToSearch = "a" };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, FieldsToSearch = "a" };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsTrue(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Different_FieldsToSearch_Is_Not_Equal()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, FieldsToSearch = "A" };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, FieldsToSearch = "ass" };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsFalse(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Same_StartIndex_IsEqual()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, StartIndex = "a" };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, StartIndex = "a" };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsTrue(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Different_StartIndex_Is_Not_Equal()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, StartIndex = "A" };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, StartIndex = "ass" };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsFalse(@equals);
        }



        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Same_MatchCase_IsEqual()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, MatchCase = true };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, MatchCase = true };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsTrue(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Different_MatchCase_Is_Not_Equal()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, MatchCase = false };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, MatchCase = true };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsFalse(@equals);
        }

        
        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Same_RequireAllTrue_IsEqual()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, RequireAllTrue = true };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, RequireAllTrue = true };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsTrue(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Different_RequireAllTrue_Is_Not_Equal()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, RequireAllTrue = false };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, RequireAllTrue = true };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsFalse(@equals);
        }


        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Same_RequireAllFieldsToMatch_IsEqual()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, RequireAllFieldsToMatch = true };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, RequireAllFieldsToMatch = true };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsTrue(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Different_RequireAllFieldsToMatch_Is_Not_Equal()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, RequireAllFieldsToMatch = false };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, RequireAllFieldsToMatch = true };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsFalse(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Same_ResultsCollection_IsEqual()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var resultsColl = new List<FindRecordsTO>();
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, ResultsCollection = resultsColl };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, ResultsCollection = resultsColl };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsTrue(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Different_ResultsCollection_Is_Not_Equal()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var resultsColl = new List<FindRecordsTO>
            {
                new FindRecordsTO
                {
                    SearchType = "SOMETHING"
                }
            };
            var resultsColl2 = new List<FindRecordsTO>
            {
                new FindRecordsTO
                {
                    SearchType = "NONE"
                }
            };
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, ResultsCollection = resultsColl };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, ResultsCollection = resultsColl2 };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsFalse(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Sanele Mthembu")]
        public void Equals_Given_Same_ResultsCollection_Different_Indexes_Is_Not_Equal()
        {
            //---------------Set up test pack-------------------
            var uniqueId = Guid.NewGuid().ToString();
            var resultsColl = new List<FindRecordsTO>
            {
                new FindRecordsTO
                {
                    SearchType = "SOMETHING"
                }
                ,
                new FindRecordsTO
                {
                    SearchType = "NONE"
                }

            };
            var resultsColl2 = new List<FindRecordsTO>
            {
                new FindRecordsTO
                {
                    SearchType = "NONE"
                }
                ,
                new FindRecordsTO
                {
                    SearchType = "SOMETHING"
                }
            };
            var activity1 = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, ResultsCollection = resultsColl };
            var activity = new DsfFindRecordsMultipleCriteriaActivity() { UniqueID = uniqueId, ResultsCollection = resultsColl2 };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(activity1);
            //---------------Execute Test ----------------------
            var @equals = activity1.Equals(activity);
            //---------------Test Result -----------------------
            Assert.IsFalse(@equals);
        }

        [TestMethod]
        [Timeout(60000)]
        [Owner("Hagashen Naidu")]
        [TestCategory("DsfFindRecordsMultipleCriteriaActivity_GetState")]
        public void DsfFindRecordsMultipleCriteriaActivity_GetState_ReturnsStateVariable()
        {
            var resultsCol = new List<FindRecordsTO>
            {
                new FindRecordsTO
                {
                    SearchType = "SOMETHING"
                }
                ,
                new FindRecordsTO
                {
                    SearchType = "NONE"
                }

            };
            
            var act = new DsfFindRecordsMultipleCriteriaActivity() { ResultsCollection = resultsCol,FieldsToSearch="[[rec().a]]",RequireAllTrue=true,RequireAllFieldsToMatch=false,Result="[[match]]" };

            //------------Execute Test---------------------------
            var stateItems = act.GetState();
            Assert.AreEqual(5, stateItems.Count());

            var expectedResults = new[]
            {                
                new StateVariable
                {
                    Name="ResultsCollection",
                    Type = StateVariable.StateType.Input,
                    Value = ActivityHelper.GetSerializedStateValueFromCollection(resultsCol)
                },
                new StateVariable
                {
                    Name="FieldsToSearch",
                    Type = StateVariable.StateType.Input,
                    Value = "[[rec().a]]"
                },
                new StateVariable
                {
                    Name="RequireAllTrue",
                    Type = StateVariable.StateType.Input,
                    Value = "True"
                },
                new StateVariable
                {
                    Name="RequireAllFieldsToMatch",
                    Type = StateVariable.StateType.Input,
                    Value = "False"
                },
                new StateVariable
                {
                    Name="Result",
                    Type = StateVariable.StateType.Output,
                    Value = "[[match]]"
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