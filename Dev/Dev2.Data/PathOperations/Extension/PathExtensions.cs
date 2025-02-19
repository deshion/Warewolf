/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Dev2.Data.Interfaces;

namespace Dev2.Data.PathOperations.Extension
{
    public static class PathExtensions
    {
        public static string Combine(this IActivityIOOperationsEndPoint endpoint, string with)
        {
            if (endpoint.IOPath.Path.EndsWith(endpoint.PathSeperator()))
            {
                return endpoint.IOPath.Path + with;
            }
            return endpoint.IOPath.Path + endpoint.PathSeperator() + with;
        }
    }
}