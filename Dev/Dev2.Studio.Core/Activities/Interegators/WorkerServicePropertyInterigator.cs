#pragma warning disable
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
using System.Xml;
using Dev2.Common.Common;
using Dev2.Studio.Interfaces;
using Unlimited.Applications.BusinessDesignStudio.Activities;


namespace Dev2.Studio.Core.Activities.Interegators
{
    public static class WorkerServicePropertyInterigator
    {
        public static void SetActivityProperties(IContextualResourceModel resource, ref DsfActivity activity, IResourceRepository resourceRepository)
        {
            activity.IsWorkflow = false;

            if(resource.WorkflowXaml != null && resource.WorkflowXaml.Length > 0)
            {
                var startIdx = resource.WorkflowXaml.IndexOf("<Action ", 0, true);

                if(startIdx >= 0)
                {
                    var endIdx = resource.WorkflowXaml.IndexOf(">", startIdx, true);
                    if(endIdx > 0)
                    {
                        activity = SetActivityProperties(activity, resource, resourceRepository, startIdx, endIdx);
                    }
                }
            }
            activity.Type = resource.ServerResourceType;
        }

        static DsfActivity SetActivityProperties(DsfActivity activity, IContextualResourceModel resource, IResourceRepository resourceRepository, int startIdx, int endIdx)
        {
            var len = endIdx - startIdx + 1;
            var fragment = resource.WorkflowXaml.Substring(startIdx, len);

            fragment += "</Action>";
            fragment = fragment.Replace("&", "&amp;");
            var document = new XmlDocument();

            document.LoadXml(fragment);

            if (document.DocumentElement != null)
            {
                var node = document.SelectSingleNode("//Action");
                if (node?.Attributes != null)
                {
                    activity = SetActivityProperties(activity, resourceRepository, node);
                }
            }
            return activity;
        }

        static DsfActivity SetActivityProperties(DsfActivity activity, IResourceRepository resourceRepository, XmlNode node)
        {
            var attr = node.Attributes["SourceName"];
            if (attr != null)
            {
                if (resourceRepository != null && node.Attributes["SourceID"] != null)
                {
                    Guid.TryParse(node.Attributes["SourceID"].Value, out Guid sourceId);
                    activity.FriendlySourceName = resourceRepository.LoadContextualResourceModel(sourceId).DisplayName;
                }
                else
                {
                    activity.FriendlySourceName = attr.Value;
                }
            }

            attr = node.Attributes["SourceMethod"];
            if (attr != null)
            {
                activity.ActionName = attr.Value;
            }
            return activity;
        }
    }
}
