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
using System.Linq;
using Dev2.Data.Util;
using Dev2.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using Unlimited.Applications.BusinessDesignStudio.Activities;
using Warewolf.Tools.Specs.BaseTypes;

namespace Dev2.Activities.Specs.Toolbox.Data.FindIndex
{
    [Binding]
    public class FindIndexSteps : RecordSetBases
    {
        public FindIndexSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
        }

        protected override void BuildDataList()
        {
            scenarioContext.TryGetValue("variableList", out List<Tuple<string, string>> variableList);

            if (variableList == null)
            {
                variableList = new List<Tuple<string, string>>();
                scenarioContext.Add("variableList", variableList);
            }

            variableList.Add(new Tuple<string, string>(ResultVariable, ""));
            var resultVariable = ResultVariable;
            if (scenarioContext.TryGetValue("resultVariable", out string resultVar))
            {
                resultVariable = resultVar;
                var resultVars = resultVariable.Split(',');
                variableList.AddRange(Enumerable.Select<string, Tuple<string, string>>(resultVars, resVar => new Tuple<string, string>(resVar, "")));
            }
            BuildShapeAndTestData();

            scenarioContext.TryGetValue("inField", out string inField);
            scenarioContext.TryGetValue("index", out string index);
            scenarioContext.TryGetValue("characters", out string characters);
            scenarioContext.TryGetValue("direction", out string direction);

            var findIndex = new DsfIndexActivity
                {
                    Result = resultVariable,
                    InField = inField,
                    Index = index,
                    Characters = characters,
                    Direction = direction
                };

            TestStartNode = new FlowStep
                {
                    Action = findIndex
                };
            scenarioContext.Add("activity", findIndex);
        }

        [Given(@"the sentence ""(.*)""")]
        public void GivenTheSentence(string inField)
        {
            scenarioContext.Add("inField", inField);
        }

        [Given(@"I selected Index ""(.*)""")]
        [Given(@"I have selected Index ""(.*)""")]
        public void GivenISelectedIndex(string index)
        {
            scenarioContext.Add("index", index);
        }

       
        [Given(@"I search for characters ""(.*)""")]
        public void GivenISearchForCharacters(string characters)
        {
            if(characters == "\" \"")
            {
                characters = ' '.ToString();
            }
            scenarioContext.Add("characters", characters);
        }

        [Given(@"I selected direction as ""(.*)""")]
        public void GivenISelectedDirectionAs(string direction)
        {
            scenarioContext.Add("direction", direction);
        }

        [Given(@"I have a Find Index variable ""(.*)"" equal to ""(.*)""")]
        public void GivenIHaveAFindIndexVariableEqualTo(string variable, string value)
        {
            scenarioContext.TryGetValue("variableList", out List<Tuple<string, string>> variableList);

            if (variableList == null)
            {
                variableList = new List<Tuple<string, string>>();
                scenarioContext.Add("variableList", variableList);
            }

            variableList.Add(new Tuple<string, string>(variable, value));
        }

        [Given(@"a find index recordset")]
        public void GivenAFindIndexRecordset(Table table)
        {
            var records = table.Rows.ToList();

            if (records.Count == 0)
            {
                var rs = table.Header.ToArray()[0];
                var field = table.Header.ToArray()[1];


                var isAdded = scenarioContext.TryGetValue("rs", out List<Tuple<string, string>> emptyRecordset);
                if (!isAdded)
                {
                    emptyRecordset = new List<Tuple<string, string>>();
                    scenarioContext.Add("rs", emptyRecordset);
                }
                emptyRecordset.Add(new Tuple<string, string>(rs, field));
            }

            foreach (TableRow record in records)
            {
                scenarioContext.TryGetValue("variableList", out List<Tuple<string, string>> variableList);

                if (variableList == null)
                {
                    variableList = new List<Tuple<string, string>>();
                    scenarioContext.Add("variableList", variableList);
                }
                variableList.Add(new Tuple<string, string>(record[0], record[1]));
            }
        }

        [When(@"the data find index tool is executed")]
        public void WhenTheDataFindIndexToolIsExecuted()
        {
            BuildDataList();
            var result = ExecuteProcess(isDebug: true, throwException: false);
            scenarioContext.Add("result", result);
        }

        [Then(@"the find index result is ""(.*)""")]
        public void ThenTheFindIndexResultIs(string results)
        {
            if (string.IsNullOrEmpty(results))
            {
                results = null;
            }
            else
            {
                results = results.Replace("\"\"", "");
            }
            var result = scenarioContext.Get<IDSFDataObject>("result");
            GetScalarValueFromEnvironment(result.Environment, DataListUtil.RemoveLanguageBrackets(ResultVariable),
                                       out string actualValue, out string error);
            Assert.AreEqual(results, actualValue);
        }

        [Then(@"the find index result is")]
        public void ThenTheFindIndexResultIs(Table table)
        {
            var result = scenarioContext.Get<IDSFDataObject>("result");
            GetScalarValueFromEnvironment(result.Environment, DataListUtil.RemoveLanguageBrackets(ResultVariable),
                                       out string actualValue, out string error);

            var records = actualValue.Split(',').ToList();
            var tableRows = table.Rows.ToList();

            Assert.AreEqual(tableRows.Count, records.Count);

            for (int i = 0; i < tableRows.Count; i++)
            {
                Assert.AreEqual(tableRows[i][0], records[i]);
            }
        }

        [Given(@"result variable as ""(.*)""")]
        public void GivenResultVariableAs(string resultVar)
        {
            scenarioContext.Add("resultVariable", resultVar);
        }
    }
}
