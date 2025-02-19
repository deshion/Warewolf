#pragma warning disable
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
using System.Xml.Linq;
using Dev2.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Interfaces.Core.Graph;
using Dev2.Common.Utils;
using Dev2.Data.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Unlimited.Framework.Converters.Graph;

namespace Dev2.Runtime.ServiceModel.Data
{
    public class WebService : Service, IDisposable
    {
        bool _disposed;

        public string RequestUrl { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public WebRequestMethod RequestMethod { get; set; }

        public string RequestHeaders { get; set; }
        public string RequestBody { get; set; }
        public string RequestResponse { get; set; }

        public RecordsetList Recordsets { get; set; }
        public string JsonPath { get; set; }
        public string RequestMessage { get; set; }
        public string JsonPathResult { get; set; }

        public List<INameValue> Headers { get; set; }
        public List<IFormDataParameters> FormDataParameters { get; internal set; }
        public bool IsFormDataChecked { get; set; }
        public bool IsUrlEncodedChecked { get; set; }
        public bool IsManualChecked { get; set; }

        public WebService()
        {
            ResourceID = Guid.Empty;
            ResourceType = "WebService";
            Source = new WebSource();
            Recordsets = new RecordsetList();
            Method = new ServiceMethod();
        }

        public WebService(XElement xml)
            : base(xml)
        {
            ResourceType = "WebService";
            var action = xml.Descendants("Action").FirstOrDefault();
            if(action == null)
            {
                
                if (xml.HasAttributes && xml.Attribute("Type").Value == "InvokeWebService")
                {
                    action = xml;
                }
                else
                {
                    return;
                }
            }

            RequestUrl = action.AttributeSafe("RequestUrl");
            JsonPath = action.AttributeSafe("JsonPath");
            RequestMethod = Enum.TryParse(action.AttributeSafe("RequestMethod"), true, out WebRequestMethod requestMethod) ? requestMethod : WebRequestMethod.Get;
            RequestHeaders = action.ElementSafe("RequestHeaders");
            RequestBody = action.ElementSafe("RequestBody");

            Source = CreateSource<WebSource>(action);
            Method = CreateInputsMethod(action);
            Recordsets = CreateOutputsRecordsetList(action);
        }

        public override XElement ToXml()
        {
            var result = CreateXml(enActionType.InvokeWebService, Source, Recordsets,
                new XAttribute("RequestUrl", RequestUrl ?? string.Empty),
                new XAttribute("RequestMethod", RequestMethod.ToString()),
                new XAttribute("JsonPath", JsonPath ?? string.Empty),
                new XElement("RequestHeaders", new XCData(RequestHeaders ?? string.Empty)),
                new XElement("RequestBody", new XCData(RequestBody ?? string.Empty))
                );
            return result;
        }

        public IOutputDescription GetOutputDescription()
        {
            IOutputDescription result = null;
            var dataSourceShape = DataSourceShapeFactory.CreateDataSourceShape();

            var requestResponse = Scrubber.Scrub(RequestResponse);

            try
            {
                result = OutputDescriptionFactory.CreateOutputDescription(OutputFormats.ShapedXML);
                dataSourceShape = DataSourceShapeFactory.CreateDataSourceShape();
                result.DataSourceShapes.Add(dataSourceShape);
                var dataBrowser = DataBrowserFactory.CreateDataBrowser();
                dataSourceShape.Paths.AddRange(dataBrowser.Map(requestResponse));
            }

            catch(Exception ex)
            {
                var dataBrowser = DataBrowserFactory.CreateDataBrowser();
                var errorResult = new XElement("Error");
                errorResult.Add(ex);
                var data = errorResult.ToString();
                dataSourceShape.Paths.AddRange(dataBrowser.Map(data));
            }
            return result;
        }

        // This destructor will run only if the Dispose method does not get called.
        ~WebService()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }

        // Implement IDisposable. 
        // Do not make this method virtual. 
        // A derived class should not be able to override this method. 
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios. 
        // If disposing equals true, the method has been called directly 
        // or indirectly by a user's code. Managed and unmanaged resources 
        // can be disposed. 
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed. 
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if(!_disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                // Dispose managed resources.
                if (disposing && Source != null)
                {
                    ((WebSource)Source).Dispose();
                    Source = null;
                }


                // Call the appropriate methods to clean up 
                // unmanaged resources here. 

                // Note disposing has been done.
                _disposed = true;
            }
        }

        public void ApplyPath()
        {
            if(String.IsNullOrEmpty(RequestResponse) || String.IsNullOrEmpty(JsonPath))
            {
                return;
            }

            JsonPath = JsonPath.Trim();

            try
            {
                var json = JObject.Parse(RequestResponse);
                var context = new JsonPathContext { ValueSystem = new JsonNetValueSystem() };
                var values = context.SelectNodes(json, JsonPath).Select(node => node.Value);
                var newResponseValue = JsonConvert.SerializeObject(values);

                JsonPathResult = newResponseValue;
            }
            catch(JsonException je)
            {
                Dev2Logger.Error(je, GlobalConstants.WarewolfError);
            }
        }
    }
}
