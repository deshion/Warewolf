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

using System.ComponentModel;
using System.Windows.Input;

namespace Dev2.Common.Interfaces.Data
{
    public interface IInputOutputViewModel : INotifyPropertyChanged
    {
        string Name { get; set; }
        bool IsSelected { get; set; }
        string Value { get; set; }
        string MapsTo { get; set; }
        string DefaultValue { get; set; }
        bool Required { get; set; }
        string RecordSetName { get; set; }
        string DisplayName { get; set; }
        string DisplayDefaultValue { get; }
        bool IsNew { get; set; }
        bool RequiredMissing { get; set; }
        string TypeName { get; set; }
        bool IsMapsToFocused { get; set; }
        bool IsValueFocused { get; set; }
        ICommand ViewComplexObjectsCommand { get; set; }
        bool IsObject { get; set; }

        IDev2Definition GetGenerationTO();
    }
}