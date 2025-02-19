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
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Dev2.Common.Interfaces.Scheduler.Interfaces;


namespace Dev2.AppResources.Converters
{
    public class SchedulerStatusToCheckedConverter : DependencyObject, IValueConverter
    {
        #region Override Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var schedulerStatus = (SchedulerStatus)value;

            if (schedulerStatus == SchedulerStatus.Enabled)
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isEnabledRadioButton = parameter as string;
            if (isEnabledRadioButton == "true" && (bool)value || isEnabledRadioButton == "false" && !(bool)value)
            {
                return SchedulerStatus.Enabled;
            }
            return SchedulerStatus.Disabled;
        }

        #endregion Override Mehods
    }
}
