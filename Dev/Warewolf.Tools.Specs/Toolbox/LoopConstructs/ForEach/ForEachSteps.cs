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
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Enums;
using Dev2.Communication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using Unlimited.Applications.BusinessDesignStudio.Activities;
using Warewolf.Tools.Specs.BaseTypes;
using Dev2.Common.Interfaces;
using Dev2.Data.TO;
using Dev2.Interfaces;
using Dev2.Data.Interfaces.Enums;
using Dev2.DataList.Contract;
using Warewolf.Storage.Interfaces;

namespace Dev2.Activities.Specs.Toolbox.LoopConstructs.ForEach
{
    [Binding]
    public class ForEachSteps : RecordSetBases
    {
        public ForEachSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
        }

        const string ResultRecordsetVariable = "[[r().v]]";

        protected override void BuildDataList()
        {
            scenarioContext.TryGetValue("variableList", out List<Tuple<string, string>> variableList);

            if (variableList == null)
            {
                variableList = new List<Tuple<string, string>>();
                scenarioContext.Add("variableList", variableList);
            }

            variableList.Add(new Tuple<string, string>(ResultRecordsetVariable, ""));

            if (scenarioContext.TryGetValue("outMapTo", out string outMapTo))
            {
                variableList.Add(new Tuple<string, string>(outMapTo, ""));
            }

            BuildShapeAndTestData();

            var activityType = scenarioContext.Get<string>("activityType");

            dynamic activity;

            if(activityType.Equals("Tool"))
            {
                activity = new DsfRandomActivity
                    {
                        Result = ResultRecordsetVariable,
                        RandomType = enRandomType.Numbers,
                        From = "0",
                        To = "100"
                    };
            }
            else
            {
                activity = new DsfActivity
                    {
                        InputMapping = BuildInputMappings(),
                        OutputMapping = BuildOutputMappings(),
                        ServiceName = "SpecflowForeachActivityTest"
                    };
            }

            var activityFunction = new ActivityFunc<string, bool> { Handler = activity };
            var foreachType = scenarioContext.Get<enForEachType>("foreachType");

            if (!scenarioContext.TryGetValue("recordset", out string recordSet))
            {
                recordSet = string.Empty;
            }

            if (!scenarioContext.TryGetValue("from", out string from))
            {
                from = string.Empty;
            }

            if (!scenarioContext.TryGetValue("to", out string to))
            {
                to = string.Empty;
            }

            if (!scenarioContext.TryGetValue("numberAs", out string numberAs))
            {
                numberAs = string.Empty;
            }

            var dsfForEach = new DsfForEachActivity
                {
                    ForEachType = foreachType,
                    Recordset = recordSet,
                    From = from,
                    To = to,
                    CsvIndexes = numberAs,
                    NumOfExections = numberAs,
                    DataFunc = activityFunction
                };

            TestStartNode = new FlowStep
                {
                    Action = dsfForEach
                };

            scenarioContext.Add("activity", dsfForEach);
        }

        string BuildInputMappings()
        {
            var inputMappings = new StringBuilder();
            inputMappings.Append("<Inputs>");

            var inMapTo = scenarioContext.Get<string>("inMapTo");
            var inRecordset = RetrieveItemForEvaluation(enIntellisensePartType.RecordsetsOnly, inMapTo);
            var inColumn = RetrieveItemForEvaluation(enIntellisensePartType.RecordsetFields, inMapTo);

            var inMapFrom = scenarioContext.Get<string>("inMapFrom");
            inputMappings.Append(string.Format("<Input Name=\"{0}\" Source=\"{1}\" Recordset=\"{2}\"/>", inColumn,
                                               inMapFrom, inRecordset));

            inputMappings.Append("</Inputs>");
            return inputMappings.ToString();
        }

        string BuildOutputMappings()
        {
            var outputMappings = new StringBuilder();
            outputMappings.Append("<Outputs>");

            var outMapFrom = scenarioContext.Get<string>("outMapFrom");
            var inRecordset = RetrieveItemForEvaluation(enIntellisensePartType.RecordsetsOnly, outMapFrom);
            var inColumn = RetrieveItemForEvaluation(enIntellisensePartType.RecordsetFields, outMapFrom);

            var outMapTo = scenarioContext.Get<string>("outMapTo");
            outputMappings.Append(string.Format(
                "<Output Name=\"{0}\" MapsTo=\"{1}\" Value=\"{1}\" Recordset=\"{2}\"/>", inColumn,
                outMapTo, inRecordset));

            outputMappings.Append("</Outputs>");
            return outputMappings.ToString();
        }

        [Given(@"I have selected the foreach type as ""(.*)"" and used ""(.*)""")]
        public void GivenIHaveSelectedTheForeachTypeAsAndUsed(string foreachType, string recordSet)
        {
            var forEachType = (enForEachType)Enum.Parse(typeof(enForEachType), foreachType);
            scenarioContext.Add("foreachType", forEachType);
            switch (forEachType)
            {
                case enForEachType.NumOfExecution:
                    scenarioContext.Add("numberAs", recordSet);
                    break;
                case enForEachType.InRange:
                    scenarioContext.Add("from", recordSet);
                    scenarioContext.Add("to", recordSet);
                    break;
                case enForEachType.InCSV:
                    scenarioContext.Add("numberAs", recordSet);
                    break;
                case enForEachType.InRecordset:
                    scenarioContext.Add("recordset", recordSet);
                    break;
                default:
                    break;
            }
        }

        [Given(@"I have selected the foreach type as ""(.*)"" from (.*) to (.*)")]
        public void GivenIHaveSelectedTheForeachTypeAsFromTo(string foreachType, string from, string to)
        {
            scenarioContext.Add("foreachType", (enForEachType)Enum.Parse(typeof(enForEachType), foreachType));
            scenarioContext.Add("from", from);
            scenarioContext.Add("to", to);
        }

        [Given(@"I have selected the foreach type as ""(.*)"" as ""(.*)""")]
        public void GivenIHaveSelectedTheForeachTypeAsAs(string foreachType, string numberAs)
        {
            scenarioContext.Add("foreachType", (enForEachType)Enum.Parse(typeof(enForEachType), foreachType));
            scenarioContext.Add("numberAs", numberAs);
        }

        [Given(@"the underlying dropped activity is a\(n\) ""(.*)""")]
        public void GivenTheUnderlyingDroppedActivityIsAN(string activityType)
        {
            scenarioContext.Add("activityType", activityType);
        }

        [When(@"the foreach tool is executed")]
        public void WhenTheForeachToolIsExecuted()
        {
            BuildDataList();
            var result = ExecuteProcess(isDebug: true, throwException: false, channel: new mockEsb(scenarioContext));
            scenarioContext.Add("result", result);
        }
        
        [Then(@"the foreach executes (.*) times")]
        public void ThenTheForeachExecutesTimes(int numOfIterations)
        {
            var recordset = RetrieveItemForEvaluation(enIntellisensePartType.RecordsetsOnly, ResultRecordsetVariable);
            var column = RetrieveItemForEvaluation(enIntellisensePartType.RecordsetFields, ResultRecordsetVariable);
            var result = scenarioContext.Get<IDSFDataObject>("result");
            var recordSetValues = RetrieveAllRecordSetFieldValues(DataObject.Environment, recordset, column, out string error);
            recordSetValues = Enumerable.Where<string>(recordSetValues, i => !string.IsNullOrEmpty(i)).ToList();
            Assert.AreEqual<int>(numOfIterations, recordSetValues.Count);
        }
    }

    public class mockEsb : IEsbChannel
    {
        readonly ScenarioContext scenarioContext;

        public mockEsb(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException("scenarioContext");
            }

            this.scenarioContext = scenarioContext;
        }

        #region Not Implemented

        public Guid ExecuteRequest(IDSFDataObject dataObject, EsbExecuteRequest request, Guid workspaceID,
                                   out ErrorResultTO errors)
        {
            errors = new ErrorResultTO();
            return Guid.NewGuid();
        }

        public T FetchServerModel<T>(IDSFDataObject dataObject, Guid workspaceID, out ErrorResultTO errors, int update)
        {
            throw new NotImplementedException();
        }

        public string FindServiceShape(Guid workspaceID, string serviceName, int update)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the service shape.
        /// </summary>
        /// <param name="workspaceID">The workspace unique identifier.</param>
        /// <param name="resourceID">Name of the service.</param>
        /// <returns></returns>
        public StringBuilder FindServiceShape(Guid workspaceID, Guid resourceID)
        {
            return null;
        }

        public IList<KeyValuePair<enDev2ArgumentType, IList<IDev2Definition>>> ShapeForSubRequest(
            IDSFDataObject dataObject, string inputDefs, string outputDefs, out ErrorResultTO errors)
        {
            throw new NotImplementedException();
        }

        public Guid CorrectDataList(IDSFDataObject dataObject, Guid workspaceID, out ErrorResultTO errors,
                                    IDataListCompiler compiler)
        {
            throw new NotImplementedException();
        }

        public void ExecuteLogErrorRequest(IDSFDataObject dataObject, Guid workspaceID, string uri,
                                           out ErrorResultTO errors, int update)
        {
            throw new NotImplementedException();
        }

        public IExecutionEnvironment UpdatePreviousEnvironmentWithSubExecutionResultUsingOutputMappings(IDSFDataObject dataObject, string outputDefs, int update, bool handleErrors, ErrorResultTO errors)
        {
            return null;
        }

        public void CreateNewEnvironmentFromInputMappings(IDSFDataObject dataObject, string inputDefs, int update)
        {
        }

        #endregion

        public IExecutionEnvironment ExecuteSubRequest(IDSFDataObject dataObject, Guid workspaceID, string inputDefs, string outputDefs,
                                      out ErrorResultTO errors, int update,bool b)
        {
            if (!scenarioContext.TryGetValue("indexUpdate", out List<int> updateValues))
            {
                updateValues = new List<int>();
                scenarioContext.Add("indexUpdate", updateValues);
            }

            if (!scenarioContext.TryGetValue("inputDefs", out List<string> inputList))
            {
                inputList = new List<string>();
                scenarioContext.Add("inputDefs", inputList);
            }

            if(!scenarioContext.TryGetValue("outputDefs", out List<string> outputList))
            {
                outputList = new List<string>();
                scenarioContext.Add("outputDefs", outputList);
            }

            inputList.Add(inputDefs);
            outputList.Add(outputDefs);
            updateValues.Add(update);
            errors = new ErrorResultTO();
            return dataObject.Environment;
        }
    }
}
