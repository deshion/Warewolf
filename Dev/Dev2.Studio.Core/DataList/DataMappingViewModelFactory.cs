/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Collections.Specialized;
using Dev2.Studio.Interfaces;

namespace Dev2.Studio.ViewModels.DataList
{
    public class DataMappingViewModelFactory:IDataMappingViewModelFactory
    {
        public IDataMappingViewModel CreateModel(IWebActivity activity)=>CreateModel(activity, null);
        public IDataMappingViewModel CreateModel(IWebActivity activity, NotifyCollectionChangedEventHandler mappingCollectionChangedEventHandler) => new DataMappingViewModel(activity, mappingCollectionChangedEventHandler);
    }
}
