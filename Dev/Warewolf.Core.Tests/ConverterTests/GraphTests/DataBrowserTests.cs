/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2021 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Dev2.Common.Interfaces.Core.Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unlimited.Framework.Converters.Graph;
using Unlimited.Framework.Converters.Graph.Poco;
using Unlimited.Framework.Converters.Graph.String.Json;
using Unlimited.Framework.Converters.Graph.String.Xml;
using Unlimited.UnitTest.Framework.ConverterTests.GraphTests;


namespace Dev2.Tests.ConverterTests.GraphTests
{
    [TestClass]
    [TestCategory(nameof(DataBrowserFactory))]
    public class DataBrowserTests
    {
        internal PocoTestData GivenPoco()
        {
            var testData = new PocoTestData
            {
                Name = "Brendon",
                Age = 30,
                NestedData = new PocoTestData
                {
                    Name = "AnotherBrendon",
                    Age = 31,
                },
            };

            var nestedTestData1 = new PocoTestData
            {
                Name = "Mo",
                Age = 30,
                NestedData = new PocoTestData
                {
                    Name = "AnotherMo",
                    Age = 31,
                },
            };

            var nestedTestData2 = new PocoTestData
            {
                Name = "Trav",
                Age = 30,
                NestedData = new PocoTestData
                {
                    Name = "AnotherTrav",
                    Age = 31,
                },
            };

            testData.EnumerableData = new List<PocoTestData> { nestedTestData1, nestedTestData2 };

            return testData;
        }

        internal PocoTestData GivenPocoWithParallelAndNestedEnumerables()
        {
            var testData = new PocoTestData
            {
                Name = "Brendon",
                Age = 30,
                NestedData = new PocoTestData
                {
                    Name = "AnotherBrendon",
                    Age = 31,
                },
            };

            var nestedTestData1 = new PocoTestData
            {
                Name = "Mo",
                Age = 30,
                NestedData = new PocoTestData
                {
                    Name = "AnotherMo",
                    Age = 31,
                },
            };

            var nestedTestData2 = new PocoTestData
            {
                Name = "Trav",
                Age = 30,
                NestedData = new PocoTestData
                {
                    Name = "AnotherTrav",
                    Age = 31,
                },
            };

            var nestedTestData3 = new PocoTestData
            {
                Name = "Jayd",
                Age = 30,
                NestedData = new PocoTestData
                {
                    Name = "AnotherJayd",
                    Age = 31,
                },
            };

            var nestedTestData4 = new PocoTestData
            {
                Name = "Dan",
                Age = 30,
                NestedData = new PocoTestData
                {
                    Name = "AnotherDan",
                    Age = 31,
                },
            };

            var nestedTestData5 = new PocoTestData
            {
                Name = "Mark",
                Age = 30,
                NestedData = new PocoTestData
                {
                    Name = "AnotherMark",
                    Age = 31,
                },
            };

            var nestedTestData6 = new PocoTestData
            {
                Name = "Warren",
                Age = 30,
                NestedData = new PocoTestData
                {
                    Name = "AnotherWarren",
                    Age = 31,
                },
            };

            var nestedTestData7 = new PocoTestData
            {
                Name = "Wallis",
                Age = 30,
                NestedData = new PocoTestData
                {
                    Name = "AnotherWallis",
                    Age = 31,
                },
            };

            nestedTestData1.EnumerableData = new List<PocoTestData> { nestedTestData3, nestedTestData4, nestedTestData6 };
            nestedTestData2.EnumerableData = new List<PocoTestData> { nestedTestData5 };

            testData.EnumerableData = new List<PocoTestData> { nestedTestData1, nestedTestData2 };
            testData.EnumerableData1 = new List<PocoTestData> { nestedTestData3, nestedTestData4, nestedTestData5, nestedTestData6, nestedTestData7 };

            return testData;
        }

        internal string GivenXml()
        {
            return @"<Company Name='Dev2'>
    <Motto>Eat lots of cake</Motto>
    <PreviousMotto/>
	<Departments TestAttrib='testing'>
		<Department Name='Dev'>
			<Employees>
				<Person Name='Brendon' Surename='Page' />
				<Person Name='Jayd' Surename='Page' />
			</Employees>
		</Department>
		<Department Name='Accounts'>
			<Employees>
				<Person Name='Bob' Surename='Soap' />
				<Person Name='Joe' Surename='Pants' />
			</Employees>
		</Department>
	</Departments>
    <InlineRecordSet>
        RandomData
    </InlineRecordSet>
    <InlineRecordSet>
        RandomData1
    </InlineRecordSet>
    <OuterNestedRecordSet>
        <InnerNestedRecordSet ItemValue='val1' />
        <InnerNestedRecordSet ItemValue='val2' />
    </OuterNestedRecordSet>
    <OuterNestedRecordSet>
        <InnerNestedRecordSet ItemValue='val3' />
        <InnerNestedRecordSet ItemValue='val4' />
    </OuterNestedRecordSet>
</Company>";
        }

        internal string GivenJson()
        {
            return @"{
    ""Name"": ""Dev2"",
    ""Motto"": ""Eat lots of cake"",
    ""Departments"": [      
        {
          ""Name"": ""Dev"",
          ""Employees"": [
              {
                ""Name"": ""Brendon"",
                ""Surename"": ""Page""
              },
              {
                ""Name"": ""Jayd"",
                ""Surename"": ""Page""
              }
            ]
        },
        {
          ""Name"": ""Accounts"",
          ""Employees"": [
              {
                ""Name"": ""Bob"",
                ""Surename"": ""Soap""
              },
              {
                ""Name"": ""Joe"",
                ""Surename"": ""Pants""
              }
            ]
        }
      ],
    ""Contractors"": [      
        {
          ""Name"": ""Roofs Inc."",
          ""PhoneNumber"": ""123"",
        },
        {
          ""Name"": ""Glass Inc."",
          ""PhoneNumber"": ""1234"",
        },
        {
          ""Name"": ""Doors Inc."",
          ""PhoneNumber"": ""1235"",
        },
        {
          ""Name"": ""Cakes Inc."",
          ""PhoneNumber"": ""1236"",
        }
      ],
    ""PrimitiveRecordset"": [
      ""
        RandomData
    "",
      ""
        RandomData1
    ""
    ],
  }";
        }

        /// <summary>
        /// Map paths of unexpected type expected poco paths returned.
        /// </summary>
        [TestMethod]
        public void DataBrowserFactory_Map_PathsOfUnexpectedType_Expected_PocoPaths()
        {
            var uri = new Uri("/cake", UriKind.Relative);

            var dataBrowser = DataBrowserFactory.CreateDataBrowser();
            var paths = dataBrowser.Map(uri);

            Assert.IsTrue(paths.All(p => p.GetType() == typeof(PocoPath)));
        }

        /// <summary>
        /// Map paths of reference type expected poco paths returned.
        /// </summary>
        [TestMethod]
        public void DataBrowserFactory_Map_GivenPoco_MapPathsOfReferenceType_Expected_PocoPaths()
        {
            var testData = GivenPoco();

            var dataBrowser = DataBrowserFactory.CreateDataBrowser();
            var paths = dataBrowser.Map(testData);

            Assert.IsTrue(paths.All(p => p.GetType() == typeof(PocoPath)));
        }

        [TestMethod]
        [Owner("Siphamandla Dube")]
        public void DataBrowserFactory_Map_GivenJson_PathsOfReferenceType_Expected_JsonPaths()
        {
            var dataBrowser = DataBrowserFactory.CreateDataBrowser();
            var paths = dataBrowser.Map(GivenJson());

            Assert.IsTrue(paths.All(p => p.GetType() == typeof(JsonPath)));
        }

        /// <summary>
        /// Select scalar value using poco scalar path from reference type expected scalar value returned.
        /// </summary>
        [TestMethod]
        public void DataBrowserFactory_SelectScalar_ValueUsingPocoScalarPathFromReferenceType_Expected_ScalarValue()
        {
            var testData = GivenPoco();

            IPath namePath = new PocoPath("Name", "Name");
            var dataBrowser = DataBrowserFactory.CreateDataBrowser();

            var data = dataBrowser.SelectScalar(namePath, testData);

            Assert.AreEqual(data, testData.Name);
        }

        /// <summary>
        /// Select enumerable value using poco enumerable path from reference type expected values from each item 
        /// in enumeration.
        /// </summary>
        [TestMethod]
        public void DataBrowserFactory_SelectEnumerable_ValueUsingPocoEnumerablePathFromReferenceType_Expected_ValuesFromEachItemInEnumeration()
        {
            var testData = GivenPoco();

            IPath namePath = new PocoPath("EnumerableData().Name", "EnumerableData.Name");
            var dataBrowser = DataBrowserFactory.CreateDataBrowser();

            var data = dataBrowser.SelectEnumerable(namePath, testData);

            var expected = string.Join("|", testData.EnumerableData.Select(e => e.Name));
            var actual = string.Join("|", data.Select(o => o.ToString()));

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Select enumerable values as related using poco enumerable paths from reference type where paths contain nested enumerable paths 
        /// expected flattened data with values from outer enumerable path repeating for every value from nested enumerable path.
        /// </summary>
        [TestMethod]
        public void DataBrowserFactory_SelectEnumerable_ValuesAsRelatedUsingPocoEnumerablePathsFromReferenceType_Where_PathsContainNestedEnumerablePaths_Expected_FlattenedDataWithValuesFromOuterEnumerablePathRepeatingForEveryValueFromNestedEnumerablePath()
        {
            var testData = GivenPocoWithParallelAndNestedEnumerables();

            var enumerableNamePath = new PocoPath("EnumerableData().Name", "EnumerableData.Name");
            var nestedEnumerableNamePath = new PocoPath("EnumerableData().EnumerableData().Name", "EnumerableData.EnumerableData.Name");
            var paths = new List<IPath> { enumerableNamePath, nestedEnumerableNamePath };

            var dataBrowser = DataBrowserFactory.CreateDataBrowser();
            var data = dataBrowser.SelectEnumerablesAsRelated(paths, testData);

            #region Complex Setup for Expected

            //
            // The code in this region is used to setup the exprected value.
            // It can't be reused for other tests and can't be made generic
            // without replicating the funcationality being tested.
            //
            var tmpExpected = "";
            var tmpExpected1 = "";
            var separator = "|";

            for (int outerCount = 0; outerCount < testData.EnumerableData.Count; outerCount++)
            {
                for (int innerCount = 0; innerCount < testData.EnumerableData[outerCount].EnumerableData.Count; innerCount++)
                {
                    if (outerCount == testData.EnumerableData.Count - 1 && innerCount == testData.EnumerableData[outerCount].EnumerableData.Count - 1)
                    {
                        separator = "";
                    }

                    if (outerCount < testData.EnumerableData.Count)
                    {
                        tmpExpected += testData.EnumerableData[outerCount].Name + separator;
                    }
                    else
                    {
                        tmpExpected += separator;
                    }

                    if (innerCount < testData.EnumerableData[outerCount].EnumerableData.Count)
                    {
                        tmpExpected1 += testData.EnumerableData[outerCount].EnumerableData[innerCount].Name + separator;
                    }
                    else
                    {
                        tmpExpected1 += separator;
                    }
                }
            }

            #endregion Complex Setup for Expected

            var expected = tmpExpected + "^" + tmpExpected1;
            var actual = string.Join("|", data[enumerableNamePath]);
            actual += "^" + string.Join("|", data[nestedEnumerableNamePath]);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Map paths of XML expected XML paths returned.
        /// </summary>
        [TestMethod]
        public void DataBrowserFactory_Map_PathsOfXml_Expected_XmlPaths()
        {
            var testData = GivenXml();

            var dataBrowser = DataBrowserFactory.CreateDataBrowser();
            var paths = dataBrowser.Map(testData);

            Assert.IsTrue(paths.All(p => p.GetType() == typeof(XmlPath)));
        }

        /// <summary>
        /// Select scalar value using poco scalar path from XML expected scalar value returned.
        /// </summary>
        [TestMethod]
        public void DataBrowserFactory_SelectScalar_ValueUsingPocoScalarPathFromXml_Expected_ScalarValue()
        {
            var testData = GivenXml();

            IPath namePath = new XmlPath("Company:Name", "Company:Name");
            var dataBrowser = DataBrowserFactory.CreateDataBrowser();

            var data = dataBrowser.SelectScalar(namePath, testData);

            Assert.AreEqual(data, "Dev2");
        }

        /// <summary>
        /// Select enumerable values as related using poco enumerable paths from XML where paths contain nested 
        /// enumerable paths expected flattened data with values from outer 
        /// enumerable path repeating for every value from nested enumerable path.
        /// </summary>
        [TestMethod]
        public void DataBrowserFactory_SelectEnumerable_ValuesAsRelatedUsingPocoEnumerablePathsFromXml_Where_PathsContainNestedEnumerablePaths_Expected_FlattenedDataWithValuesFromOuterEnumerablePathRepeatingForEveryValueFromNestedEnumerablePath()
        {
            var testData = GivenXml();

            var enumerableNamePath = new XmlPath("Company.Departments().Department:Name", "Company.Departments.Department:Name");
            var nestedEnumerableNamePath = new XmlPath("Company.Departments().Department.Employees().Person:Name", "Company.Departments.Department.Employees.Person:Name");
            var paths = new List<IPath> { enumerableNamePath, nestedEnumerableNamePath };

            var dataBrowser = DataBrowserFactory.CreateDataBrowser();
            var data = dataBrowser.SelectEnumerablesAsRelated(paths, testData);

            const string expected = "Dev|Dev|Accounts|Accounts^Brendon|Jayd|Bob|Joe";
            var actual = string.Join("|", data[enumerableNamePath]);
            actual += "^" + string.Join("|", data[nestedEnumerableNamePath]);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Map paths of JSON expected JSON paths.
        /// </summary>
        [TestMethod]
        public void DataBrowserFactory_Map_PathsOfJson_Expected_JsonPaths()
        {
            var testData = GivenJson();

            var dataBrowser = DataBrowserFactory.CreateDataBrowser();
            var paths = dataBrowser.Map(testData);

            Assert.IsTrue(paths.All(p => p.GetType() == typeof(JsonPath)));
        }

        /// <summary>
        /// Select scalar value using JSON scalar path from JSON expected scalar value returned.
        /// </summary>
        [TestMethod]
        public void DataBrowserFactory_SelectScalar_ValueUsingJsonScalarPathFromJson_Expected_ScalarValue()
        {
            var testData = GivenJson();

            IPath namePath = new JsonPath("Name", "Name");
            var dataBrowser = DataBrowserFactory.CreateDataBrowser();

            var data = dataBrowser.SelectScalar(namePath, testData);

            Assert.AreEqual(data, "Dev2");
        }

        /// <summary>
        /// Select enumerable values as related using JSON enumerable paths from json where paths contain 
        /// nested enumerable paths expected flattened data with values from outer enumerable path repeating 
        /// for every value from nested enumerable path.
        /// </summary>
        [TestMethod]
        public void DataBrowserFactory_SelectEnumerable_ValuesAsRelatedUsingJsonEnumerablePathsFromJson_Where_PathsContainNestedEnumerablePaths_Expected_FlattenedDataWithValuesFromOuterEnumerablePathRepeatingForEveryValueFromNestedEnumerablePath()
        {
            var testData = GivenJson();

            IPath enumerableNamePath = new JsonPath("Departments().Name", "Departments.Name");
            IPath nestedEnumerableNamePath = new JsonPath("Departments().Employees().Name", "Departments.Employees.Name");
            var paths = new List<IPath> { enumerableNamePath, nestedEnumerableNamePath };

            var dataBrowser = DataBrowserFactory.CreateDataBrowser();
            var data = dataBrowser.SelectEnumerablesAsRelated(paths, testData);

            const string expected = "Dev|Dev|Accounts|Accounts^Brendon|Jayd|Bob|Joe";
            var actual = string.Join("|", data[enumerableNamePath]);
            actual += "^" + string.Join("|", data[nestedEnumerableNamePath]);

            Assert.AreEqual(expected, actual);
        }
    }
}
