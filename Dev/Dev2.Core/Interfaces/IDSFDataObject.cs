#pragma warning disable
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
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Diagnostics.Debug;
using Dev2.Data.Interfaces.Enums;
using Dev2.Web;
using Warewolf.Storage.Interfaces;
using System.Collections.Concurrent;
using Dev2.Common.Interfaces.Enums;
using Warewolf;
using Warewolf.Auditing;
using Warewolf.Data;

namespace Dev2.Interfaces
{
    public interface ICoverageDataObject
    {
        EmitionTypes ReturnType { get; }
        Guid ResourceID { get; }
        IWarewolfWorkflow[] CoverageReportResources { get; set; }
        bool IsMultipleWorkflowReport { get; }
        string GetTestUrl(string resourcePath);
        string GetAllTestsUrl();
        string ReportName { get; set; }

    }

    public class RetryState
    {
        public int NumberOfRetries { get; set; }
    }

    public interface IDSFDataObject : IExecutionContext
    {
        Dictionary<int, List<Guid>> ThreadsToDispose { get; set; }
        ConcurrentDictionary<(IPrincipal, AuthorizationContext, string), bool> AuthCache { get; set; }

        string CurrentBookmarkName { get; set; }
        Guid WorkflowInstanceId { get; set; }
        string ServiceName { get; set; }
        string TestName { get; set; }
        bool IsServiceTestExecution { get; set; }
        bool IsDebugFromWeb { get; set; }
        string ParentWorkflowInstanceId { get; set; }
        string ParentServiceName { get; set; }
        StringBuilder DataList { get; set; }
        bool WorkflowResumeable { get; set; }
        bool IsDebug { get; set; }
        Guid WorkspaceID { get; set; }
        Guid ResourceID { get; set; }
        Guid SourceResourceID { get; set; }
        Guid OriginalInstanceID { get; set; }
        Guid ClientID { get; set; }
        bool IsOnDemandSimulation { get; set; }
        Guid ServerID { get; set; }
        int VersionNumber { get; set; }
        int NumberOfSteps { get; set; }
        IPrincipal ExecutingUser { get; set; }
        Guid DatalistOutMergeID { get; set; }
        enDataListMergeTypes DatalistOutMergeType { get; set; }
        enTranslationDepth DatalistOutMergeDepth { get; set; }
        DataListMergeFrequency DatalistOutMergeFrequency { get; set; }

        Guid DatalistInMergeID { get; set; }
        enDataListMergeTypes DatalistInMergeType { get; set; }
        enTranslationDepth DatalistInMergeDepth { get; set; }

        Guid ExecutionCallbackID { get; set; }
        Guid BookmarkExecutionCallbackID { get; set; }
        string ParentInstanceID { get; set; }

        EmitionTypes ReturnType { get; set; }

        // 2012.11.05 : Travis.Frisinger - Added for Binary DataList
        Guid DataListID { get; set; }
        bool IsDataListScoped { get; set; }
        bool ForceDeleteAtNextNativeActivityCleanup { get; set; }
        StringBuilder RawPayload { get; set; }
        StringBuilder RemoteInvokeResultShape { get; set; }
        bool RemoteInvoke { get; set; }
        //TODO: Change this to Guid
        string RemoteInvokerID { get; set; }
        IList<IDebugState> RemoteDebugItems { get; set; }
        string RemoteServiceType { get; set; }

        ExecutionOrigin ExecutionOrigin { get; set; }
        string ExecutionOriginDescription { get; set; }
        bool IsFromWebServer { get; set; }

        Guid EnvironmentID { get; set; }

        int ParentThreadID { get; set; }

        Guid DebugSessionID { get; set; }
        Guid ParentID { get; set; }

        bool RunWorkflowAsync { get; set; }
        bool IsDebugNested { get; set; }

        bool IsRemoteInvoke { get; }
        bool IsRemoteInvokeOverridden { get; set; }

        int ForEachNestingLevel { get; set; }
        IDSFDataObject Clone();
        bool IsDebugMode();
        bool IsRemoteWorkflow();
        IExecutionEnvironment Environment { get; set; }
        IExecutionToken ExecutionToken { get; set; }

        void PopEnvironment();
        void PushEnvironment(IExecutionEnvironment env);

        IEsbChannel EsbChannel { get; set; }
        int ForEachUpdateValue { get; set; }
        DateTime StartTime { get; set; }
        Guid DebugEnvironmentId { get; set; }
        bool RemoteNonDebugInvoke { get; set; }
        bool StopExecution { get; set; }
        IServiceTestModelTO ServiceTest { get; set; }
        Guid[] TestsResourceIds { get; set; }
        Guid? ExecutionID { get; set; }
        string CustomTransactionID { get; set; }
        string WebUrl { get; set; }
        bool IsSubExecution { get; set; }
        string QueryString { get; set; }

        IDev2WorkflowSettings Settings { get; set; }
        IStateNotifier StateNotifier { get; set; }
        Exception ExecutionException { get; set; }
        IDictionary<IDev2Activity, (RetryState, IEnumerator<bool>)> Gates { get; }
        string OriginalServiceName { get; set; }
        IWarewolfResource Resource { get; set; }
        Guid StartActivityId { get; set; }
    }
}
