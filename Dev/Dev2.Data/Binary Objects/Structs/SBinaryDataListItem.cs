
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;

namespace Dev2.DataList.Contract.Binary_Objects.Structs
{
    [Serializable]
    public struct SBinaryDataListItem
    {

        public string TheValue { get; set; }

        public int ItemCollectionIndex { get; set; }

        public string Namespace { get; set; }

        public string FieldName { get; set; }

        public string DisplayValue { get; set; }

    }
}
