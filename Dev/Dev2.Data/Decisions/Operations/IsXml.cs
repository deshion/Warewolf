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
using Dev2.Data.Util;

namespace Dev2.Data.Decisions.Operations
{
    public class IsXml : IDecisionOperation
    {
        public Enum HandlesType() => enDecisionType.IsXML;

        public bool Invoke(string[] cols)
        {
            var data = DataListUtil.AdjustForEncodingIssues(cols[0]);
            var isXml = DataListUtil.IsXml(data, out bool isFragment);
            return isXml || isFragment;
        }
    }
}
