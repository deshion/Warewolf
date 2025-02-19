/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2021 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Activities;
using System.Collections.Generic;
using Dev2.Activities.Debug;
using Dev2.Common;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Common.State;
using Dev2.Comparer;
using Dev2.Data.TO;
using Dev2.Interfaces;
using Dev2.Util;
using Unlimited.Applications.BusinessDesignStudio.Activities;
using Warewolf.Auditing;
using Warewolf.Common.NetStandard20;
using Warewolf.Core;
using Warewolf.Driver.Persistence;
using Warewolf.Execution;
using Warewolf.Resource.Errors;
using Warewolf.Streams;

namespace Dev2.Activities
{
    [ToolDescriptorInfo("ControlFlow-ManualResumption", "Manual Resumption", ToolType.Native, "8999E58B-38A3-43BB-A98F-6090C5C9EA1F", "Dev2.Activities", "1.0.0.0", "Legacy", "Control Flow", "/Warewolf.Studio.Themes.Luna;component/Images.xaml", "Tool_Flow_ManualResumption")]
    public class ManualResumptionActivity : DsfBaseActivity, IEquatable<ManualResumptionActivity>, IStateNotifierRequired
    {
        private IDSFDataObject _dataObject;
        private int _update;
        private IStateNotifier _stateNotifier;
        private readonly bool _persistenceEnabled;
        private readonly IPersistenceExecution _scheduler;
        private readonly IExecutionLogPublisher _logger;

        public ManualResumptionActivity()
            : this(Config.Persistence, new PersistenceExecution(),
                new ExecutionLogger.ExecutionLoggerFactory().New(new JsonSerializer(), new WebSocketPool()))
        {
        }

        public ManualResumptionActivity(PersistenceSettings config, IPersistenceExecution resumeExecution, IExecutionLogPublisher logger)
        {
            DisplayName = "Manual Resumption";
            OverrideDataFunc = new ActivityFunc<string, bool>
            {
                DisplayName = "Data Action",
                Argument = new DelegateInArgument<string>($"explicitData_{DateTime.Now:yyyyMMddhhmmss}"),
                Handler = new DsfSequenceActivity(),
            };
            _persistenceEnabled = config.Enable;
            _scheduler = resumeExecution;
            _logger = logger;
        }

        /// <summary>
        /// The property that holds the result string the user enters into the "SuspensionId" box
        /// </summary>
        [FindMissing]
        public string SuspensionId { get; set; }

        [FindMissing] public string Response { get; set; }

        /// <summary>
        /// The property that holds the result bool the user selects from the "OverrideInputVariables" box
        /// </summary>
        [FindMissing]
        public bool OverrideInputVariables { get; set; }


        public ActivityFunc<string, bool> OverrideDataFunc { get; set; }

        public override IEnumerable<StateVariable> GetState()
        {
            return new[]
            {
                new StateVariable
                {
                    Name = nameof(Response),
                    Value = Response,
                    Type = StateVariable.StateType.Output
                },
                new StateVariable
                {
                    Name = nameof(SuspensionId),
                    Value = SuspensionId,
                    Type = StateVariable.StateType.Input
                },
                new StateVariable
                {
                    Name = nameof(OverrideInputVariables),
                    Value = OverrideInputVariables.ToString(),
                    Type = StateVariable.StateType.Input
                }
            };
        }

        protected override void OnExecute(NativeActivityContext context)
        {
            var dataObject = context.GetExtension<IDSFDataObject>();
            _dataObject = dataObject;
            ExecuteTool(_dataObject, 0);
        }

        protected override void ExecuteTool(IDSFDataObject dataObject, int update)
        {
            _dataObject = dataObject;
            _update = update;
            base.ExecuteTool(_dataObject, update);
        }

        protected override List<string> PerformExecution(Dictionary<string, string> evaluatedValues)
        {
            var allErrors = new ErrorResultTO();
            Response = string.Empty;
            try
            {
                var suspensionId = EvalSuspensionId();
                if (string.IsNullOrWhiteSpace(suspensionId))
                {
                    throw new Exception(ErrorResource.ManualResumptionSuspensionIdBlank);
                }

                if (!_persistenceEnabled)
                {
                    throw new Exception(ErrorResource.PersistenceSettingsNoConfigured);
                }

                _logger.Info("Performing Resume of job {" + suspensionId + "}, connection established.", suspensionId);
                
                const string OverrideVariables = "";
                if (OverrideInputVariables)
                {
                    var persistedValues = _scheduler.GetPersistedValues(suspensionId);
                    if (string.IsNullOrEmpty(persistedValues.SuspendedEnvironment))
                    {
                        throw new Exception(ErrorResource.ManualResumptionSuspensionEnvBlank);
                    }

                    if (persistedValues.SuspendedEnvironment.StartsWith("Failed:"))
                    {
                        throw new Exception(persistedValues.SuspendedEnvironment);
                    }
                    var envArray = _dataObject.Environment.ToJson();
                    var resumeObject = _dataObject;
                    resumeObject.StartActivityId = persistedValues.StartActivityId;
                    resumeObject.Environment.FromJson(persistedValues.SuspendedEnvironment);
                    resumeObject.Environment.FromJson(envArray);
                    resumeObject.ExecutingUser = persistedValues.ExecutingUser;
                    InnerActivity(resumeObject, _update);
                    Response = _scheduler.ManualResumeWithOverrideJob(resumeObject, suspensionId);
                }
                else
                {
                    Response = _scheduler.ResumeJob(_dataObject, suspensionId, OverrideInputVariables, OverrideVariables);
                }

                _stateNotifier?.LogActivityExecuteState(this);
                if (_dataObject.IsDebugMode())
                {
                    var debugItemStaticDataParams = new DebugItemStaticDataParams("SuspensionID: " + suspensionId, "", true);
                    AddDebugOutputItem(debugItemStaticDataParams);
                    debugItemStaticDataParams = new DebugItemStaticDataParams("Override Variables: " + OverrideInputVariables, "", true);
                    AddDebugOutputItem(debugItemStaticDataParams);
                    debugItemStaticDataParams = new DebugItemStaticDataParams("Result: " + Response, "", true);
                    AddDebugOutputItem(debugItemStaticDataParams);
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                LogException(new Exception(ErrorResource.BackgroundJobClientResumeFailed), allErrors);
            }
            catch (Exception ex)
            {
                LogException(ex, allErrors);
            }
            finally
            {
                HandleErrors(_dataObject, allErrors);
            }

            return new List<string> {Response};
        }

        private void LogException(Exception ex, ErrorResultTO allErrors)
        {
            _stateNotifier?.LogExecuteException(ex, this);
            Dev2Logger.Error(DisplayName, ex, GlobalConstants.WarewolfError);
            _dataObject.ExecutionException = ex;
            allErrors.AddError(ex.Message);
        }

        void HandleErrors(IDSFDataObject data, ErrorResultTO allErrors)
        {
            var hasErrors = allErrors.HasErrors();
            if (!hasErrors)
            {
                return;
            }
            foreach (var errorString in allErrors.FetchErrors())
            {
                data.Environment.AddError(errorString);
            }
            DisplayAndWriteError(data, DisplayName, allErrors);
        }

        public override enFindMissingType GetFindMissingType() => enFindMissingType.ManualResumption;

        private string EvalSuspensionId()
        {
            var debugEvalResult = new DebugEvalResult(SuspensionId, nameof(SuspensionId), _dataObject.Environment, _update);
            AddDebugInputItem(debugEvalResult);
            var suspensionId = string.Empty;

            var debugItemResults = debugEvalResult.GetDebugItemResult();
            if (debugItemResults.Count > 0)
            {
                suspensionId = debugItemResults[0].Value;
            }

            return suspensionId.Trim();
        }

        private void InnerActivity(IDSFDataObject dataObject, int update)
        {
            if (OverrideDataFunc.Handler is DsfSequenceActivity sequenceActivity)
            {
                foreach (var dsfActivity in sequenceActivity.Activities)
                {
                    if (dsfActivity is IDev2Activity act)
                    {
                        ExecuteActivity(dataObject, update, act);
                    }
                }
            }
        }

        public override IEnumerable<IDev2Activity> GetChildrenNodes()
        {
            var act = OverrideDataFunc.Handler as IDev2ActivityIOMapping;
            if (act == null)
            {
                return new List<IDev2Activity>();
            }
            var childNodes = new List<IDev2Activity> { act };
            return childNodes;
        }

        private static void ExecuteActivity(IDSFDataObject dataObject, int update, IDev2Activity act)
        {
            act.Execute(dataObject, update);
        }

        public void SetStateNotifier(IStateNotifier stateNotifier)
        {
            if (_stateNotifier is null)
            {
                _stateNotifier = stateNotifier;
            }
        }

        public bool Equals(ManualResumptionActivity other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            var activityFuncComparer = new ActivityFuncComparer();
            var equals = base.Equals(other);
            equals &= Equals(SuspensionId, other.SuspensionId);
            equals &= Equals(OverrideInputVariables, other.OverrideInputVariables);
            equals &= Equals(Response, other.Response);
            equals &= activityFuncComparer.Equals(OverrideDataFunc, other.OverrideDataFunc);
            return equals;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ManualResumptionActivity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (SuspensionId != null ? SuspensionId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ OverrideInputVariables.GetHashCode();
                hashCode = (hashCode * 397) ^ (Response != null ? Response.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OverrideDataFunc != null ? OverrideDataFunc.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}