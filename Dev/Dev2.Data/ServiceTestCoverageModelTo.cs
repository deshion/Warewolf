﻿/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2021 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using Warewolf.Data;
using Dev2.Common.Interfaces;
using System.Collections.Generic;

namespace Dev2.Data
{
    public class ServiceTestCoverageModelTo : IServiceTestCoverageModelTo
    {
        public ISingleTestNodesCovered[] AllTestNodesCovered { get; set; }
        public IEnumerable<IWorkflowNode> AllWorkflowNodes { get; set; } = new List<IWorkflowNode>(); 

        public string OldReportName { get; set; }

        public string ReportName { get; set; }

        public Guid WorkflowId { get; set; }

        public DateTime LastRunDate { get; set; }

        public double TotalCoverage { get; set; }

        public int NotCoveredNodesCount { get; set; }
    }

}
