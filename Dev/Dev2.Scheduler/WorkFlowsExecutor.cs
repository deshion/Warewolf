
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
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
using System.Threading.Tasks;
using Dev2.Scheduler.Interfaces;

namespace Dev2.Scheduler
{
    public class WorkFlowsExecutor : IWorkflowExecutor
    {
        public IResourceHistory RunWorkFlow(string serverURI, string workflowName)
        {
           throw new NotImplementedException();
        }
    }
}
