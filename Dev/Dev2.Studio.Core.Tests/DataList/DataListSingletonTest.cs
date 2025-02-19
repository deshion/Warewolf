/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Threading;
using Dev2.Studio.Core;
using Dev2.Studio.Interfaces.DataList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Dev2.Core.Tests
{
    [TestClass]
    [TestCategory("Studio Datalist Core")]
    public class DataListSingletonTest
    {
        public static readonly object DataListSingletonTestGuard = new object();

        public TestContext TestContext { get; set; }

        #region Additional test attributes

        [TestInitialize]
        public void Init()
        {
            Monitor.Enter(DataListSingletonTestGuard);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Monitor.Exit(DataListSingletonTestGuard);
        }

        #endregion

        #region SetDataList Tests

        [TestMethod]
        public void SetDataList_Expected_CurrentDataListSetInSingleton()
        {
            var mockdataListViewModel = Dev2MockFactory.SetupDataListViewModel();
            DataListSingleton.SetDataList(mockdataListViewModel.Object);
            Assert.AreEqual(DataListSingleton.ActiveDataList, mockdataListViewModel.Object);
        }

        #endregion SetDataList Tests

        #region UpdateActiveDataList Tests

        [TestMethod]
        public void UpdateActiveDataList_Expected_NewActiveDataList()
        {
            var mockdataListViewModel = Dev2MockFactory.SetupDataListViewModel();
            DataListSingleton.SetDataList(mockdataListViewModel.Object);
            var mock_newDataListViewModel = new Mock<IDataListViewModel>();
            mock_newDataListViewModel.Setup(dataList => dataList.Resource).Returns(Dev2MockFactory.SetupResourceModelMock().Object);
            DataListSingleton.SetDataList(mock_newDataListViewModel.Object);
            Assert.AreNotEqual(DataListSingleton.ActiveDataList, mockdataListViewModel);
        }

        #endregion UpdateActiveDataList Tests
    }
}
