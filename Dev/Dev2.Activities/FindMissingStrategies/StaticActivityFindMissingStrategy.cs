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
using System.Collections.Generic;
using System.Reflection;
using Dev2.Interfaces;
using Dev2.Util;
using Dev2.Utilities;


namespace Dev2.FindMissingStrategies
{
    public class StaticActivityFindMissingStrategy : IFindMissingStrategy
    {
        #region Implementation of ISpookyLoadable<Enum>

        public Enum HandlesType() => enFindMissingType.StaticActivity;
        
        public List<string> GetActivityFields(object activity)
        {
            var results = new List<string>();
            var properties = StringAttributeRefectionUtils.ExtractAdornedProperties<FindMissingAttribute>(activity);
            foreach (PropertyInfo propertyInfo in properties)
            {
                var property = propertyInfo.GetValue(activity, null);
                if (property != null)
                {
                    results.Add(property.ToString());
                }
            }

            return results;
        }

        #endregion
    }
}
