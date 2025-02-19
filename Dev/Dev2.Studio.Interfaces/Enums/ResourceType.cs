/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dev2.Studio.Interfaces.Enums
{
    public enum ResourceType
    {
        [Description("Workflow service")]
        [Display(Order = 1)]
        WorkflowService,

        [Description("Worker Service")]
        [Display(Order = 2)]
        Service,

        [Description("Source")]
        [Display(Order = 3)]
        Source,

        [Description("Unknown")]
        Unknown,

        Server
    }

    public static class ResourceTypeExtensions
    {
        public static WorkSurfaceContext ToWorkSurfaceContext(this ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.WorkflowService:
                    return WorkSurfaceContext.Workflow;
                case ResourceType.Service:
                    return WorkSurfaceContext.Service;
                case ResourceType.Source:
                    return WorkSurfaceContext.SourceManager;
                case ResourceType.Unknown:
                default:
                    return WorkSurfaceContext.Unknown;
            }
        }

    }
}
