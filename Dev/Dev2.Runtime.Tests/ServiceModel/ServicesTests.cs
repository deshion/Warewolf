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
using System.Xml.Linq;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Tests.Runtime.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Dev2.Tests.Runtime.ServiceModel
{

    /// <author>trevor.williams-ros</author>
    /// <date>2013/02/13</date>
    [TestClass]
    [TestCategory("Runtime Hosting")]
    public class ServicesTests
    {
        #region CreateInputsMethod

        [TestMethod]
        [Owner("Travis Frisinger")]
        [TestCategory("Service_CreateInputsMethods")]
        public void Service_CreateInputsMethods_Plugin_EmptyType()
        {
            //------------Setup for test--------------------------
            var service = new Service();

            #region Test String

            const string input = @"<Action Name=""EmitStringData"" Type=""Plugin"" SourceName=""Anything To Xml Hook Plugin"" SourceMethod=""EmitStringData"" NativeType=""String"">
  <Inputs>
    <Input Name=""StringData"" Source=""StringData"" DefaultValue="""" NativeType="""">
      <Validator Type=""Required"" />
    </Input>
  </Inputs>
  <Outputs>
    <Output Name=""CompanyName"" MapsTo=""CompanyName"" Value=""[[Names().CompanyName]]"" Recordset=""Names"" />
    <Output Name=""DepartmentName"" MapsTo=""DepartmentName"" Value=""[[Names().DepartmentName]]"" Recordset=""Names"" />
    <Output Name=""EmployeeName"" MapsTo=""EmployeeName"" Value=""[[Names().EmployeeName]]"" Recordset=""Names"" />
  </Outputs>
  <OutputDescription><![CDATA[<z:anyType xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:d1p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.Ouput"" i:type=""d1p1:OutputDescription"" xmlns:z=""http://schemas.microsoft.com/2003/10/Serialization/"">
                <d1p1:DataSourceShapes xmlns:d2p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                  <d2p1:anyType i:type=""d1p1:DataSourceShape"">
                    <d1p1:Paths>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().CompanyName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev2</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Eat lots of cake</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">testing</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().DepartmentName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev,Accounts</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().EmployeeName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Brendon,Jayd</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Surename</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Surename</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Page,Page</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">
    RandomData
   ,
    RandomData1
   </SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">val1,val2</SampleData>
                      </d2p1:anyType>
                    </d1p1:Paths>
                  </d2p1:anyType>
                </d1p1:DataSourceShapes>
                <d1p1:Format>ShapedXML</d1p1:Format>
              </z:anyType>]]></OutputDescription>
</Action>";

            #endregion

            //------------Execute Test---------------------------

            var xe = XElement.Parse(input);
            var sm = service.CreateInputsMethod(xe);

            //------------Assert Results-------------------------

            Assert.AreEqual(typeof(object).FullName, sm.Parameters[0].TypeName);
        }

        [TestMethod]
        [Owner("Travis Frisinger")]
        [TestCategory("Service_CreateInputsMethods")]
        public void Service_CreateInputsMethods_Plugin_StringType()
        {
            //------------Setup for test--------------------------
            var service = new Service();
            
            #region Test String

            const string input = @"<Action Name=""EmitStringData"" Type=""Plugin"" SourceName=""Anything To Xml Hook Plugin"" SourceMethod=""EmitStringData"" NativeType=""String"">
  <Inputs>
    <Input Name=""StringData"" Source=""StringData"" DefaultValue="""" NativeType=""String"">
      <Validator Type=""Required"" />
    </Input>
  </Inputs>
  <Outputs>
    <Output Name=""CompanyName"" MapsTo=""CompanyName"" Value=""[[Names().CompanyName]]"" Recordset=""Names"" />
    <Output Name=""DepartmentName"" MapsTo=""DepartmentName"" Value=""[[Names().DepartmentName]]"" Recordset=""Names"" />
    <Output Name=""EmployeeName"" MapsTo=""EmployeeName"" Value=""[[Names().EmployeeName]]"" Recordset=""Names"" />
  </Outputs>
  <OutputDescription><![CDATA[<z:anyType xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:d1p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.Ouput"" i:type=""d1p1:OutputDescription"" xmlns:z=""http://schemas.microsoft.com/2003/10/Serialization/"">
                <d1p1:DataSourceShapes xmlns:d2p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                  <d2p1:anyType i:type=""d1p1:DataSourceShape"">
                    <d1p1:Paths>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().CompanyName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev2</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Eat lots of cake</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">testing</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().DepartmentName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev,Accounts</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().EmployeeName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Brendon,Jayd</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Surename</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Surename</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Page,Page</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">
    RandomData
   ,
    RandomData1
   </SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">val1,val2</SampleData>
                      </d2p1:anyType>
                    </d1p1:Paths>
                  </d2p1:anyType>
                </d1p1:DataSourceShapes>
                <d1p1:Format>ShapedXML</d1p1:Format>
              </z:anyType>]]></OutputDescription>
</Action>";

            #endregion

            //------------Execute Test---------------------------

            var xe = XElement.Parse(input);
            var sm = service.CreateInputsMethod(xe);

            //------------Assert Results-------------------------

            Assert.AreEqual(typeof(string).FullName, sm.Parameters[0].TypeName);
        }

        [TestMethod]
        [Owner("Travis Frisinger")]
        [TestCategory("Service_CreateInputsMethods")]
        public void Service_CreateInputsMethods_Plugin_NameCorrect()
        {
            //------------Setup for test--------------------------
            var service = new Service();

            #region Test String

            const string input = @"<Action Name=""EmitStringData"" Type=""Plugin"" SourceName=""Anything To Xml Hook Plugin"" SourceMethod=""EmitStringData"" NativeType=""String"">
  <Inputs>
    <Input Name=""StringData"" Source=""StringData"" DefaultValue="""" NativeType=""String"">
      <Validator Type=""Required"" />
    </Input>
  </Inputs>
  <Outputs>
    <Output Name=""CompanyName"" MapsTo=""CompanyName"" Value=""[[Names().CompanyName]]"" Recordset=""Names"" />
    <Output Name=""DepartmentName"" MapsTo=""DepartmentName"" Value=""[[Names().DepartmentName]]"" Recordset=""Names"" />
    <Output Name=""EmployeeName"" MapsTo=""EmployeeName"" Value=""[[Names().EmployeeName]]"" Recordset=""Names"" />
  </Outputs>
  <OutputDescription><![CDATA[<z:anyType xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:d1p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.Ouput"" i:type=""d1p1:OutputDescription"" xmlns:z=""http://schemas.microsoft.com/2003/10/Serialization/"">
                <d1p1:DataSourceShapes xmlns:d2p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                  <d2p1:anyType i:type=""d1p1:DataSourceShape"">
                    <d1p1:Paths>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().CompanyName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev2</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Eat lots of cake</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">testing</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().DepartmentName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev,Accounts</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().EmployeeName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Brendon,Jayd</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Surename</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Surename</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Page,Page</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">
    RandomData
   ,
    RandomData1
   </SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">val1,val2</SampleData>
                      </d2p1:anyType>
                    </d1p1:Paths>
                  </d2p1:anyType>
                </d1p1:DataSourceShapes>
                <d1p1:Format>ShapedXML</d1p1:Format>
              </z:anyType>]]></OutputDescription>
</Action>";

            #endregion

            //------------Execute Test---------------------------

            var xe = XElement.Parse(input);
            var sm = service.CreateInputsMethod(xe);

            //------------Assert Results-------------------------

            Assert.AreEqual("EmitStringData", sm.Name);
        }

        [TestMethod]
        [Owner("Travis Frisinger")]
        [TestCategory("Service_CreateInputsMethods")]
        public void Service_CreateInputsMethods_PluginEmptyToNullNotSet_EmptyToNullFalse()
        {
            //------------Setup for test--------------------------
            var service = new Service();

            #region Test String

            const string input = @"<Action Name=""EmitStringData"" Type=""Plugin"" SourceName=""Anything To Xml Hook Plugin"" SourceMethod=""EmitStringData"" NativeType=""String"">
  <Inputs>
    <Input Name=""StringData"" Source=""StringData"" DefaultValue="""" NativeType=""String"">
      <Validator Type=""Required"" />
    </Input>
  </Inputs>
  <Outputs>
    <Output Name=""CompanyName"" MapsTo=""CompanyName"" Value=""[[Names().CompanyName]]"" Recordset=""Names"" />
    <Output Name=""DepartmentName"" MapsTo=""DepartmentName"" Value=""[[Names().DepartmentName]]"" Recordset=""Names"" />
    <Output Name=""EmployeeName"" MapsTo=""EmployeeName"" Value=""[[Names().EmployeeName]]"" Recordset=""Names"" />
  </Outputs>
  <OutputDescription><![CDATA[<z:anyType xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:d1p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.Ouput"" i:type=""d1p1:OutputDescription"" xmlns:z=""http://schemas.microsoft.com/2003/10/Serialization/"">
                <d1p1:DataSourceShapes xmlns:d2p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                  <d2p1:anyType i:type=""d1p1:DataSourceShape"">
                    <d1p1:Paths>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().CompanyName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev2</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Eat lots of cake</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">testing</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().DepartmentName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev,Accounts</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().EmployeeName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Brendon,Jayd</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Surename</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Surename</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Page,Page</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">
    RandomData
   ,
    RandomData1
   </SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">val1,val2</SampleData>
                      </d2p1:anyType>
                    </d1p1:Paths>
                  </d2p1:anyType>
                </d1p1:DataSourceShapes>
                <d1p1:Format>ShapedXML</d1p1:Format>
              </z:anyType>]]></OutputDescription>
</Action>";

            #endregion

            //------------Execute Test---------------------------

            var xe = XElement.Parse(input);
            var sm = service.CreateInputsMethod(xe);

            //------------Assert Results-------------------------

            Assert.AreEqual(false, sm.Parameters[0].EmptyToNull);
        }

        [TestMethod]
        [Owner("Travis Frisinger")]
        [TestCategory("Service_CreateInputsMethods")]
        public void Service_CreateInputsMethods_PluginEmptyToNullSet_EmptyToNullTrue()
        {
            //------------Setup for test--------------------------
            var service = new Service();

            #region Test String

            const string input = @"<Action Name=""EmitStringData"" Type=""Plugin"" SourceName=""Anything To Xml Hook Plugin"" SourceMethod=""EmitStringData"" NativeType=""String"">
  <Inputs>
    <Input Name=""StringData"" Source=""StringData"" DefaultValue="""" NativeType=""String"" EmptyToNull=""true"">
      <Validator Type=""Required"" />
    </Input>
  </Inputs>
  <Outputs>
    <Output Name=""CompanyName"" MapsTo=""CompanyName"" Value=""[[Names().CompanyName]]"" Recordset=""Names"" />
    <Output Name=""DepartmentName"" MapsTo=""DepartmentName"" Value=""[[Names().DepartmentName]]"" Recordset=""Names"" />
    <Output Name=""EmployeeName"" MapsTo=""EmployeeName"" Value=""[[Names().EmployeeName]]"" Recordset=""Names"" />
  </Outputs>
  <OutputDescription><![CDATA[<z:anyType xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:d1p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.Ouput"" i:type=""d1p1:OutputDescription"" xmlns:z=""http://schemas.microsoft.com/2003/10/Serialization/"">
                <d1p1:DataSourceShapes xmlns:d2p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                  <d2p1:anyType i:type=""d1p1:DataSourceShape"">
                    <d1p1:Paths>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().CompanyName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev2</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Eat lots of cake</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">testing</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().DepartmentName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev,Accounts</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().EmployeeName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Brendon,Jayd</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Surename</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Surename</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Page,Page</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">
    RandomData
   ,
    RandomData1
   </SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">val1,val2</SampleData>
                      </d2p1:anyType>
                    </d1p1:Paths>
                  </d2p1:anyType>
                </d1p1:DataSourceShapes>
                <d1p1:Format>ShapedXML</d1p1:Format>
              </z:anyType>]]></OutputDescription>
</Action>";

            #endregion

            //------------Execute Test---------------------------

            var xe = XElement.Parse(input);
            var sm = service.CreateInputsMethod(xe);

            //------------Assert Results-------------------------

            Assert.AreEqual(true, sm.Parameters[0].EmptyToNull);
        }

        [TestMethod]
        [Owner("Travis Frisinger")]
        [TestCategory("Service_CreateInputsMethods")]
        public void Service_CreateInputsMethods_PluginRequiredNotSet_RequiredFalse()
        {
            //------------Setup for test--------------------------
            var service = new Service();

            #region Test String

            const string input = @"<Action Name=""EmitStringData"" Type=""Plugin"" SourceName=""Anything To Xml Hook Plugin"" SourceMethod=""EmitStringData"" NativeType=""String"">
  <Inputs>
    <Input Name=""StringData"" Source=""StringData"" DefaultValue="""" NativeType=""String"" EmptyToNull=""true"">
    </Input>
  </Inputs>
  <Outputs>
    <Output Name=""CompanyName"" MapsTo=""CompanyName"" Value=""[[Names().CompanyName]]"" Recordset=""Names"" />
    <Output Name=""DepartmentName"" MapsTo=""DepartmentName"" Value=""[[Names().DepartmentName]]"" Recordset=""Names"" />
    <Output Name=""EmployeeName"" MapsTo=""EmployeeName"" Value=""[[Names().EmployeeName]]"" Recordset=""Names"" />
  </Outputs>
  <OutputDescription><![CDATA[<z:anyType xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:d1p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.Ouput"" i:type=""d1p1:OutputDescription"" xmlns:z=""http://schemas.microsoft.com/2003/10/Serialization/"">
                <d1p1:DataSourceShapes xmlns:d2p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                  <d2p1:anyType i:type=""d1p1:DataSourceShape"">
                    <d1p1:Paths>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().CompanyName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev2</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Eat lots of cake</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">testing</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().DepartmentName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev,Accounts</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().EmployeeName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Brendon,Jayd</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Surename</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Surename</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Page,Page</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">
    RandomData
   ,
    RandomData1
   </SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">val1,val2</SampleData>
                      </d2p1:anyType>
                    </d1p1:Paths>
                  </d2p1:anyType>
                </d1p1:DataSourceShapes>
                <d1p1:Format>ShapedXML</d1p1:Format>
              </z:anyType>]]></OutputDescription>
</Action>";

            #endregion

            //------------Execute Test---------------------------

            var xe = XElement.Parse(input);
            var sm = service.CreateInputsMethod(xe);

            //------------Assert Results-------------------------

            Assert.AreEqual(false, sm.Parameters[0].IsRequired);
        }

        [TestMethod]
        [Owner("Travis Frisinger")]
        [TestCategory("Service_CreateInputsMethods")]
        public void Service_CreateInputsMethods_PluginRequiredSet_RequiredTrue()
        {
            //------------Setup for test--------------------------
            var service = new Service();

            #region Test String

            const string input = @"<Action Name=""EmitStringData"" Type=""Plugin"" SourceName=""Anything To Xml Hook Plugin"" SourceMethod=""EmitStringData"" NativeType=""String"">
  <Inputs>
    <Input Name=""StringData"" Source=""StringData"" DefaultValue="""" NativeType=""String"" EmptyToNull=""true"">
      <Validator Type=""Required"" />
    </Input>
  </Inputs>
  <Outputs>
    <Output Name=""CompanyName"" MapsTo=""CompanyName"" Value=""[[Names().CompanyName]]"" Recordset=""Names"" />
    <Output Name=""DepartmentName"" MapsTo=""DepartmentName"" Value=""[[Names().DepartmentName]]"" Recordset=""Names"" />
    <Output Name=""EmployeeName"" MapsTo=""EmployeeName"" Value=""[[Names().EmployeeName]]"" Recordset=""Names"" />
  </Outputs>
  <OutputDescription><![CDATA[<z:anyType xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:d1p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.Ouput"" i:type=""d1p1:OutputDescription"" xmlns:z=""http://schemas.microsoft.com/2003/10/Serialization/"">
                <d1p1:DataSourceShapes xmlns:d2p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                  <d2p1:anyType i:type=""d1p1:DataSourceShape"">
                    <d1p1:Paths>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().CompanyName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev2</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Eat lots of cake</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">testing</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().DepartmentName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev,Accounts</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().EmployeeName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Brendon,Jayd</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Surename</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Surename</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Page,Page</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">
    RandomData
   ,
    RandomData1
   </SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">val1,val2</SampleData>
                      </d2p1:anyType>
                    </d1p1:Paths>
                  </d2p1:anyType>
                </d1p1:DataSourceShapes>
                <d1p1:Format>ShapedXML</d1p1:Format>
              </z:anyType>]]></OutputDescription>
</Action>";

            #endregion

            //------------Execute Test---------------------------

            var xe = XElement.Parse(input);
            var sm = service.CreateInputsMethod(xe);

            //------------Assert Results-------------------------

            Assert.AreEqual(true, sm.Parameters[0].IsRequired);
        }

        [TestMethod]
        [Owner("Travis Frisinger")]
        [TestCategory("Service_CreateInputsMethods")]
        public void Service_CreateInputsMethods_PluginDefaultValueNotSet_DefaultValueEmpty()
        {
            //------------Setup for test--------------------------
            var service = new Service();

            #region Test String

            const string input = @"<Action Name=""EmitStringData"" Type=""Plugin"" SourceName=""Anything To Xml Hook Plugin"" SourceMethod=""EmitStringData"" NativeType=""String"">
  <Inputs>
    <Input Name=""StringData"" Source=""StringData"" DefaultValue="""" NativeType=""String"" EmptyToNull=""true"">
      <Validator Type=""Required"" />
    </Input>
  </Inputs>
  <Outputs>
    <Output Name=""CompanyName"" MapsTo=""CompanyName"" Value=""[[Names().CompanyName]]"" Recordset=""Names"" />
    <Output Name=""DepartmentName"" MapsTo=""DepartmentName"" Value=""[[Names().DepartmentName]]"" Recordset=""Names"" />
    <Output Name=""EmployeeName"" MapsTo=""EmployeeName"" Value=""[[Names().EmployeeName]]"" Recordset=""Names"" />
  </Outputs>
  <OutputDescription><![CDATA[<z:anyType xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:d1p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.Ouput"" i:type=""d1p1:OutputDescription"" xmlns:z=""http://schemas.microsoft.com/2003/10/Serialization/"">
                <d1p1:DataSourceShapes xmlns:d2p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                  <d2p1:anyType i:type=""d1p1:DataSourceShape"">
                    <d1p1:Paths>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().CompanyName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev2</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Eat lots of cake</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">testing</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().DepartmentName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev,Accounts</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().EmployeeName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Brendon,Jayd</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Surename</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Surename</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Page,Page</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">
    RandomData
   ,
    RandomData1
   </SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">val1,val2</SampleData>
                      </d2p1:anyType>
                    </d1p1:Paths>
                  </d2p1:anyType>
                </d1p1:DataSourceShapes>
                <d1p1:Format>ShapedXML</d1p1:Format>
              </z:anyType>]]></OutputDescription>
</Action>";

            #endregion

            //------------Execute Test---------------------------

            var xe = XElement.Parse(input);
            var sm = service.CreateInputsMethod(xe);

            //------------Assert Results-------------------------

            Assert.AreEqual(string.Empty, sm.Parameters[0].DefaultValue);
        }

        [TestMethod]
        [Owner("Travis Frisinger")]
        [TestCategory("Service_CreateInputsMethods")]
        public void Service_CreateInputsMethods_PluginDefaultValueSet_DefaultValueReturned()
        {
            //------------Setup for test--------------------------
            var service = new Service();

            #region Test String

            const string input = @"<Action Name=""EmitStringData"" Type=""Plugin"" SourceName=""Anything To Xml Hook Plugin"" SourceMethod=""EmitStringData"" NativeType=""String"">
  <Inputs>
    <Input Name=""StringData"" Source=""StringData"" DefaultValue=""XXX"" NativeType=""String"" EmptyToNull=""true"">
      <Validator Type=""Required"" />
    </Input>
  </Inputs>
  <Outputs>
    <Output Name=""CompanyName"" MapsTo=""CompanyName"" Value=""[[Names().CompanyName]]"" Recordset=""Names"" />
    <Output Name=""DepartmentName"" MapsTo=""DepartmentName"" Value=""[[Names().DepartmentName]]"" Recordset=""Names"" />
    <Output Name=""EmployeeName"" MapsTo=""EmployeeName"" Value=""[[Names().EmployeeName]]"" Recordset=""Names"" />
  </Outputs>
  <OutputDescription><![CDATA[<z:anyType xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:d1p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.Ouput"" i:type=""d1p1:OutputDescription"" xmlns:z=""http://schemas.microsoft.com/2003/10/Serialization/"">
                <d1p1:DataSourceShapes xmlns:d2p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                  <d2p1:anyType i:type=""d1p1:DataSourceShape"">
                    <d1p1:Paths>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().CompanyName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev2</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Motto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Eat lots of cake</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.PreviousMotto</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments:TestAttrib</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">testing</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().DepartmentName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Dev,Accounts</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Name</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Name</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">[[Names().EmployeeName]]</OutputExpression>
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Brendon,Jayd</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments().Department.Employees().Person:Surename</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.Departments.Department.Employees().Person:Surename</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Page,Page</SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().InlineRecordSet</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">
    RandomData
   ,
    RandomData1
   </SampleData>
                      </d2p1:anyType>
                      <d2p1:anyType xmlns:d5p1=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph.String.Xml"" i:type=""d5p1:XmlPath"">
                        <ActualPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company().OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</ActualPath>
                        <DisplayPath xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">Company.OuterNestedRecordSet().InnerNestedRecordSet:ItemValue</DisplayPath>
                        <OutputExpression xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"" />
                        <SampleData xmlns=""http://schemas.datacontract.org/2004/07/Unlimited.Framework.Converters.Graph"">val1,val2</SampleData>
                      </d2p1:anyType>
                    </d1p1:Paths>
                  </d2p1:anyType>
                </d1p1:DataSourceShapes>
                <d1p1:Format>ShapedXML</d1p1:Format>
              </z:anyType>]]></OutputDescription>
</Action>";

            #endregion

            //------------Execute Test---------------------------

            var xe = XElement.Parse(input);
            var sm = service.CreateInputsMethod(xe);

            //------------Assert Results-------------------------

            Assert.AreEqual("XXX", sm.Parameters[0].DefaultValue);
        }

        #endregion

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("Services_Test")]
        public void Services_Test_WhenTestingPluginHavingARecordSetFieldWithEmptyName_ExpectNotAdded()
        {
            //------------Setup for test--------------------------
            var path = UnpackDLL("PrimativesTestDLL");

            if (string.IsNullOrEmpty(path))
            {
                Assert.Fail("Failed to unpack required DLL [ PrimativesTestDLL ] ");
            }

            var services = new Dev2.Runtime.ServiceModel.Services();
            var serviceDef = JsonResource.Fetch("PrimitivePluginReturningDouble");
            var pluginService = JsonConvert.DeserializeObject<PluginService>(serviceDef, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
            pluginService.Recordsets.Add(new Recordset
            {
                Name = "Test",
                Fields = new List<RecordsetField>
                {
                    new RecordsetField
                    {
                        Name = ""                        
                    }
                }
            });
            //------------Execute Test---------------------------
            var result = services.FetchRecordset(pluginService,true);
            ////------------Assert Results-------------------------
            Assert.AreEqual(1, result[0].Fields.Count);
            StringAssert.Contains(result[0].Fields[0].Alias, "PrimitiveReturnValue");
            StringAssert.Contains(result[0].Fields[0].Name, "PrimitiveReturnValue");
            StringAssert.Contains(result[0].Fields[0].Path.ActualPath, "PrimitiveReturnValue");
            StringAssert.Contains(result[0].Fields[0].Path.SampleData, "3.1");
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("Services_Test")]
        [DoNotParallelize]
        [TestCategory("CannotParallelize")]
        public void Services_Test_WhenTestingPluginHavingARecordSetFieldNotInOutput_ExpectNotAdded()
        {
            //------------Setup for test--------------------------
            var path = UnpackDLL("PrimativesTestDLL");

            if (string.IsNullOrEmpty(path))
            {
                Assert.Fail("Failed to unpack required DLL [ PrimativesTestDLL ] ");
            }

            var services = new Dev2.Runtime.ServiceModel.Services();
            var serviceDef = JsonResource.Fetch("PrimitivePluginReturningDouble");
            var pluginService = JsonConvert.DeserializeObject<PluginService>(serviceDef, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
            pluginService.Recordsets.Add(new Recordset
            {
                Name = "Test",
                Fields = new List<RecordsetField>
                {
                    new RecordsetField
                    {
                        Name = "Bob",
                        Alias = "BobProp"
                    }
                },
                Records = new List<RecordsetRecord>
                {
                    new RecordsetRecord
                    {
                        Name = "BobRecord",
                        Label = "BobRecord"
                    }
                }
            });
            //------------Execute Test---------------------------
            var result = services.FetchRecordset(pluginService, true);
            ////------------Assert Results-------------------------
            Assert.AreEqual(1, result[0].Fields.Count);
            StringAssert.Contains(result[0].Fields[0].Alias, "PrimitiveReturnValue");
            StringAssert.Contains(result[0].Fields[0].Name, "PrimitiveReturnValue");
            StringAssert.Contains(result[0].Fields[0].Path.ActualPath, "PrimitiveReturnValue");
            StringAssert.Contains(result[0].Fields[0].Path.SampleData, "3.1");
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("Services_Test")]
        public void Services_Test_WhenWebServiceWithResponse_FetchRecordset_ExpectRecordset()
        {
            //------------Setup for test--------------------------
            
            var services = new Dev2.Runtime.ServiceModel.Services();
            var webService = new WebService();
            webService.RequestResponse = JsonConvert.SerializeObject(new { Name = "Bob", Age = 31 });
            //------------Execute Test---------------------------
            var result = services.FetchRecordset(webService, true);
            ////------------Assert Results-------------------------
            Assert.AreEqual(2, result[0].Fields.Count);
            StringAssert.Contains(result[0].Fields[0].Name, "Name");
            StringAssert.Contains(result[0].Fields[1].Name, "Age");
        }

        public string UnpackDLL(string name)
        {
            return DllExtractor.UnloadToFileSystem(name, "Plugins");
        }


    }
}
