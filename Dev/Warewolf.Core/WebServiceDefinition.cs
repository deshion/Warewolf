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
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.WebServices;

namespace Warewolf.Core
{
    public class WebServiceDefinition : IWebService    
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public IWebServiceSource Source { get; set; }
        public IList<IServiceInput> Inputs { get; set; }
        public IList<IServiceOutputMapping> OutputMappings { get; set; }
        public string QueryString { get; set; }
        public string RequestUrl { get; set; }
        public Guid Id { get; set; }
        public List<INameValue> Headers { get; set; }
        public List<INameValue> Settings { get; set; }
        public bool IsFormDataChecked { get; set; }
        public bool IsUrlEncodedChecked { get; set; }
        public bool IsManualChecked { get; set; }
        public string PostData { get; set; }
        public bool IsPutDataBase64 { get; set; }
        public string SourceUrl { get; set; }
        public string Response { get; set; }
        public WebRequestMethod Method { get; set; }

        public List<IFormDataParameters> FormDataParameters { get; set; }
    }    
}
