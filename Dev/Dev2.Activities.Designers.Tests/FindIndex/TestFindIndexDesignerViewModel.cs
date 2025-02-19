/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Activities.Presentation.Model;
using Dev2.Activities.Designers2.FindIndex;

namespace Dev2.Activities.Designers.Tests.FindIndex
{
    public class TestFindIndexDesignerViewModel : FindIndexDesignerViewModel
    {
        public TestFindIndexDesignerViewModel(ModelItem modelItem)
            : base(modelItem)
        {
        }

        public string Index { set { SetProperty(value); } get { return GetProperty<string>(); } }
        public string Direction { set { SetProperty(value); } get { return GetProperty<string>(); } }
    }
}
