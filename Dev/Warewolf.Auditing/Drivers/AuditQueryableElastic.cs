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
using System.Linq;
using System.Text;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Data.ServiceModel;
using Dev2.Runtime.ServiceModel.Data;
using Nest;
using Newtonsoft.Json.Linq;
using Warewolf.Interfaces.Auditing;
using Warewolf.Triggers;
using LogLevel = Warewolf.Logging.LogLevel;

namespace Warewolf.Auditing.Drivers
{
    public class AuditQueryableElastic : AuditQueryable, IDisposable
    {
        private string _query;
        private readonly IElasticsearchSource _elasticsearchSource;
        private readonly IElasticClient _elasticClient;

        public override string Query
        {
            get => _query;
            set => _query = value;
        }

        public AuditQueryableElastic(IElasticsearchSource elasticsearchSource, IElasticClient elasticClient)
        {
            _elasticsearchSource = elasticsearchSource;
            _elasticClient = elasticClient;
        }

        public AuditQueryableElastic(string hostname, string port, string searchIndex,
            AuthenticationType authenticationType, string username, string password)
            : this(new ElasticsearchSource(), null)
        {
            _elasticsearchSource.HostName = hostname;
            _elasticsearchSource.Port = port;
            _elasticsearchSource.SearchIndex = searchIndex;
            _elasticsearchSource.Username = username;
            _elasticsearchSource.Password = password;
            _elasticsearchSource.AuthenticationType = authenticationType;
        }

        public override IEnumerable<IExecutionHistory> QueryTriggerData(Dictionary<string, StringBuilder> values)
        {
            var resourceId = GetValue<string>("ResourceId", values);
            var result = new List<ExecutionHistory>();
            if (resourceId != null)
            {
                var jsonQuery = new JObject
                {
                    ["match"] = new JObject
                    {
                        ["fields.Data.ResourceId"] = resourceId
                    }
                };

                _query = jsonQuery.ToString();
                var results = ExecuteDatabase().ToList();
                if (results.Count > 0)
                {
                    var queryTriggerData = ExecutionHistories(results, result);
                    if (queryTriggerData != null)
                    {
                        return queryTriggerData;
                    }
                }
            }

            return result;
        }

        private static IEnumerable<IExecutionHistory> ExecutionHistories(IEnumerable<object> results, ICollection<ExecutionHistory> result)
        {
            foreach (Dictionary<string, object> item in results)
            {
                foreach (var entry in item)
                {
                    if (entry.Key != "fields")
                    {
                        continue;
                    }
                    foreach (var fields in (Dictionary<string, object>) entry.Value)
                    {
                        var executionHistory = new ExecutionHistory();

                        var keyValuePairs = ((Dictionary<string, object>) fields.Value)
                            .Where(items => items.Value != null);

                        foreach (var items in keyValuePairs)
                        {
                            switch (items.Key)
                            {
                                case "ResourceId":
                                    executionHistory.ResourceId = Guid.Parse(items.Value.ToString());
                                    break;
                                case "ExecutionInfo":
                                    var executionInfo = ExecutionInfo(items);
                                    executionHistory.ExecutionInfo = executionInfo;
                                    break;
                                case "UserName":
                                    executionHistory.UserName = items.Value.ToString();
                                    break;
                                case "Exception":
                                    executionHistory.Exception = items.Value as Exception;
                                    break;
                                case "LogLevel":
                                    Enum.TryParse((string) items.Value, true, out LogLevel logLevel);
                                    executionHistory.LogLevel = logLevel;
                                    break;
                                case "AuditType":
                                    executionHistory.AuditType = items.Value.ToString();
                                    break;
                            }
                        }

                        result.Add(executionHistory);
                    }
                }
            }

            return result;
        }

        private static ExecutionInfo ExecutionInfo(KeyValuePair<string, object> items)
        {
            var executionInfo = new ExecutionInfo();
            var keyValuePairs = ((Dictionary<string, object>) items.Value)
                .Where(infoItem => infoItem.Value != null);

            foreach (var infoItem in keyValuePairs)
            {
                switch (infoItem.Key)
                {
                    case "ExecutionId":
                        executionInfo.ExecutionId = Guid.Parse(infoItem.Value.ToString());
                        break;
                    case "CustomTransactionID":
                        executionInfo.CustomTransactionID = infoItem.Value.ToString();
                        break;
                    case "Success":
                        executionInfo.Success = items.Value is QueueRunStatus value ? value : QueueRunStatus.Success;
                        break;
                    case "EndDate":
                        executionInfo.EndDate = DateTime.Parse(infoItem.Value.ToString());
                        break;
                    case "Duration":
                        executionInfo.Duration = TimeSpan.Parse(infoItem.Value.ToString());
                        break;
                    case "StartDate":
                        executionInfo.StartDate = DateTime.Parse(infoItem.Value.ToString());
                        break;
                    case "FailureReason":
                        executionInfo.FailureReason = infoItem.Value.ToString();
                        break;
                }
            }

            return executionInfo;
        }

        public override IEnumerable<IAudit> QueryLogData(Dictionary<string, StringBuilder> values)
        {
            var result = new List<Audit>();
            BuildJsonQuery(values);
            var results = ExecuteDatabase()?.ToList();
            if (!(results?.Count > 0)) return result;
            var queryLogData = AuditLogs(results, result);
            return queryLogData ?? result;
        }

        private void BuildJsonQuery(Dictionary<string, StringBuilder> values)
        {
            var startTime = GetValue<string>("StartDateTime", values);
            var endTime = GetValue<string>("CompletedDateTime", values);
            var eventLevel = GetValue<string>("EventLevel", values);
            var executionId = GetValue<string>("ExecutionID", values);

            const string dtFormat = "yyyy-MM-ddTHH:mm:ss";

            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                if (IsUrlEncoded(startTime) & IsUrlEncoded(endTime))
                {
                    var decodedStartTime = Convert.ToDateTime(GetDecoded(startTime));
                    var decodedEndTime = Convert.ToDateTime(GetDecoded(endTime));

                    startTime = decodedStartTime.ToString(dtFormat);
                    endTime = decodedEndTime.ToString(dtFormat);
                }
                else
                {
                    startTime = Convert.ToDateTime(startTime).ToString(dtFormat);
                    endTime = Convert.ToDateTime(endTime).ToString(dtFormat);
                }
            }

            var jArray = new JArray();

            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                var dateObj = new JObject
                {
                    ["gt"] = startTime,
                    ["lt"] = endTime
                };
                var jsonQueryDateRangeFilter = new JObject
                {
                    ["range"] = new JObject
                    {
                        ["@timestamp"] = dateObj
                    }
                };
                jArray.Add(jsonQueryDateRangeFilter);
            }

            if (!string.IsNullOrEmpty(executionId))
            {
                var jsonQueryexecutionId = new JObject
                {
                    ["match"] = new JObject
                    {
                        ["fields.Data.ExecutionID"] = executionId
                    }
                };
                jArray.Add(jsonQueryexecutionId);
            }

            if (!string.IsNullOrEmpty(eventLevel))
            {
                var jsonQueryexecutionLevel = new JObject
                {
                    ["match"] = new JObject
                    {
                        ["level"] = eventLevel
                    }
                };
                jArray.Add(jsonQueryexecutionLevel);
            }

            if (jArray.Count == 0)
            {
                var matchAll = new JObject
                {
                    ["match_all"] = new JObject()
                };
                _query = matchAll.ToString();
            }
            else
            {
                var objMust = new JObject {{"must", jArray}};
                var obj = new JObject {{"bool", objMust}};
                _query = obj.ToString();
            }
        }

        private static IEnumerable<IAudit> AuditLogs(IEnumerable<object> results, ICollection<Audit> result)
        {
            try
            {
                foreach (Dictionary<string, object> item in results)
                {
                    foreach (var entry in item)
                    {
                        if (entry.Key != "fields")
                        {
                            continue;
                        }
                        foreach (var fields in (Dictionary<string, object>) entry.Value)
                        {
                            var auditHistory = new Audit();
                            var keyValuePairs = ((Dictionary<string, object>) fields.Value)
                                .Where(items => items.Value != null);

                            foreach (var items in keyValuePairs)
                            {
                                switch (items.Key)
                                {
                                    case "ExecutionID":
                                        auditHistory.ExecutionID = items.Value.ToString();
                                        break;
                                    case "CustomTransactionID":
                                        auditHistory.CustomTransactionID = items.Value.ToString();
                                        break;
                                    case "WorkflowName":
                                        auditHistory.WorkflowName = items.Value.ToString();
                                        break;
                                    case "ExecutingUser":
                                        auditHistory.ExecutingUser = items.Value.ToString();
                                        break;
                                    case "Url":
                                        auditHistory.Url = items.Value.ToString();
                                        break;
                                    case "Environment":
                                        auditHistory.Environment = string.Empty;
                                        break;
                                    case "AuditDate":
                                        auditHistory.AuditDate = DateTime.Parse(items.Value.ToString());
                                        break;
                                    case "Exception":
                                        auditHistory.Exception = items.Value as Exception;
                                        break;
                                    case "AuditType":
                                        auditHistory.AuditType = items.Value.ToString();
                                        break;
                                    case "LogLevel":
                                        Enum.TryParse((string) items.Value, true, out LogLevel logLevel);
                                        auditHistory.LogLevel = logLevel;
                                        break;
                                    case "IsSubExecution":
                                        auditHistory.IsSubExecution = bool.Parse(items.Value.ToString());
                                        break;
                                    case "IsRemoteWorkflow":
                                        auditHistory.IsRemoteWorkflow = bool.Parse(items.Value.ToString());
                                        break;
                                    case "ServerID":
                                        auditHistory.ServerID = items.Value.ToString();
                                        break;
                                    case "ParentID":
                                        auditHistory.ParentID = items.Value.ToString();
                                        break;
                                }
                            }

                            result.Add(auditHistory);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Invalid Data Returned");
            }

            return result;
        }

        private IEnumerable<object> ExecuteDatabase()
        {
            var client = _elasticClient ?? Client();
            var search = new SearchDescriptor<object>().Query(q => q.Raw(_query));
            var logEvents = client.Search<object>(search);
            var sources = logEvents.HitsMetadata?.Hits.Select(h => h.Source);
            return sources;
        }

        private ElasticClient Client()
        {
            var uri = new Uri(_elasticsearchSource.HostName + ":" + _elasticsearchSource.Port);
            var settings = new ConnectionSettings(uri)
                .RequestTimeout(TimeSpan.FromMinutes(2))
                .DefaultIndex(_elasticsearchSource.SearchIndex);
            if (_elasticsearchSource.AuthenticationType == AuthenticationType.Password)
            {
                settings.BasicAuthentication(_elasticsearchSource.Username, _elasticsearchSource.Password);
            }

            var elasticClient = new ElasticClient(settings);
            var result = elasticClient.Ping();
            var isValid = result.IsValid;
            if (!isValid)
            {
                throw result.OriginalException;
            }
            return elasticClient;
        }

        public void Dispose()
        {
            _elasticsearchSource.Dispose();
        }
    }
}