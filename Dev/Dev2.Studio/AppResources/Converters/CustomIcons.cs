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

using System.Collections.Generic;

namespace Dev2.Common
{
    public static class CustomIcons
    {
        enum PopupIcons
        {
            WarningIcon,
            InformationIcon,
            QuestionIcon,
            ErrorIcon
        }

        static readonly IDictionary<PopupIcons, string> CustomIconsDictionary = new Dictionary<PopupIcons, string>
        {
            {PopupIcons.ErrorIcon, "pack://application:,,,/Warewolf Studio;component/Images/PopupError-32.png"},
            {PopupIcons.WarningIcon, "pack://application:,,,/Warewolf Studio;component/Images/PopupNotSavedWarning-32.png"},
            {PopupIcons.QuestionIcon, "pack://application:,,,/Warewolf Studio;component/Images/GenericHelp-32.png"},
            {PopupIcons.InformationIcon, "pack://application:,,,/Warewolf Studio;component/Images/PopupInformation-32.png"}
        };

        public static string Error => CustomIconsDictionary[PopupIcons.ErrorIcon];

        public static string Information => CustomIconsDictionary[PopupIcons.InformationIcon];

        public static string Question => CustomIconsDictionary[PopupIcons.QuestionIcon];

        public static string Warning => CustomIconsDictionary[PopupIcons.WarningIcon];
    }

}
