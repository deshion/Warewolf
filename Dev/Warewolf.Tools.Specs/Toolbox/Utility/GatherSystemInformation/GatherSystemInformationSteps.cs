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
using System.Activities.Statements;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Dev2.Data.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using Warewolf.Tools.Specs.BaseTypes;
using Dev2.Common.Interfaces;
using Dev2.Data.Interfaces.Enums;
using Dev2.Interfaces;

namespace Dev2.Activities.Specs.Toolbox.Utility.GatherSystemInformation
{
    [Binding]
    public class GatherSystemInformationSteps : RecordSetBases
    {
        public GatherSystemInformationSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
        }

        protected override void BuildDataList()
        {
            BuildShapeAndTestData();

            var systemInformationCollection =
                scenarioContext.Get<List<GatherSystemInformationTO>>("systemInformationCollection");

            var dsfGatherSystemInformationActivity = new DsfGatherSystemInformationActivity
                {
                    SystemInformationCollection = systemInformationCollection
                };

            TestStartNode = new FlowStep
                {
                    Action = dsfGatherSystemInformationActivity
                };
            scenarioContext.Add("activity", dsfGatherSystemInformationActivity);
        }

        [Given(@"I have a variable ""(.*)"" and I selected ""(.*)""")]
        public void GivenIHaveAVariableAndISelected(string variable, string informationType)
        {

            var isRowAdded = scenarioContext.TryGetValue("row", out int row);
            if (isRowAdded)
            {
                scenarioContext.Add("row", row);
            }
            row++;

            scenarioContext.TryGetValue("variableList", out List<Tuple<string, string>> variableList);

            if (variableList == null)
            {
                variableList = new List<Tuple<string, string>>();
                scenarioContext.Add("variableList", variableList);
            }

            variableList.Add(new Tuple<string, string>(variable, string.Empty));
            var type =
                (enTypeOfSystemInformationToGather)
                Enum.Parse(typeof(enTypeOfSystemInformationToGather), informationType);

            scenarioContext.TryGetValue("systemInformationCollection", out List<GatherSystemInformationTO> systemInformationCollection);

            if (systemInformationCollection == null)
            {
                systemInformationCollection = new List<GatherSystemInformationTO>();
                scenarioContext.Add("systemInformationCollection", systemInformationCollection);
            }
            systemInformationCollection.Add(new GatherSystemInformationTO(type, variable, row));
        }

        [When(@"the gather system infomartion tool is executed")]
        public void WhenTheGatherSystemInfomartionToolIsExecuted()
        {
            BuildDataList();
            var result = ExecuteProcess(isDebug: true, throwException: false);
            scenarioContext.Add("result", result);
        }

        [Then(@"the value of the variable ""(.*)"" is a valid ""(.*)""")]
        public void ThenTheValueOfTheVariableIsAValid(string variable, string type)
        {
            string error;
            var result = scenarioContext.Get<IDSFDataObject>("result");

            if(DataListUtil.IsValueRecordset(variable))
            {
                var recordset = RetrieveItemForEvaluation(enIntellisensePartType.RecordsetsOnly, variable);
                var column = RetrieveItemForEvaluation(enIntellisensePartType.RecordsetFields, variable);
                var recordSetValues = RetrieveAllRecordSetFieldValues(result.Environment, recordset, column,
                                                                               out error);
                recordSetValues = recordSetValues.Where(i => !string.IsNullOrEmpty(i)).ToList();
                foreach(string recordSetValue in recordSetValues)
                {
                    Verify(type, recordSetValue, error);
                }
            }
            else
            {
                GetScalarValueFromEnvironment(result.Environment, DataListUtil.RemoveLanguageBrackets(variable),
                                           out string actualValue, out error);
                Verify(type, actualValue, error);
            }
        }

        void Verify(string type, string actualValue, string error)
        {            
            var component = Type.GetType("System." + type);
            if (component != null)
            {
                var converter = TypeDescriptor.GetConverter(component);
                converter.ConvertFrom(actualValue);
            }
            Assert.AreEqual(string.Empty, error);
        }
    }
}
