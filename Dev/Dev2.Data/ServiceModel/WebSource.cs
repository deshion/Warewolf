/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2020 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Dev2.Common.Common;
using Dev2.Common.Interfaces;
using Dev2.Data.TO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Warewolf.Common.Interfaces.NetStandard20;
using Warewolf.Security.Encryption;

namespace Dev2.Runtime.ServiceModel.Data
{
    public interface IWebPostOptions
    {
        WebSource Source { get; set; }
        WebRequestMethod Method { get; set; }
        IEnumerable<INameValue> Head { get; set; }
        IEnumerable<string> Headers { get; set; }
        IEnumerable<INameValue> Settings { get; set; }
        IEnumerable<IFormDataParameters> Parameters { get; set; }
        string Query { get; set; }
        string PostData { get; set; }
        bool IsFormDataChecked { get; set; }
        bool IsManualChecked { get; set; }
        bool IsUrlEncodedChecked { get; set; }
    }
    
    public class WebPostOptions : IWebPostOptions
    {
        public WebSource Source { get; set; }
        public WebRequestMethod Method { get; set; }
        public IEnumerable<INameValue> Head { get; set; }
        public IEnumerable<string> Headers { get; set; }
        public IEnumerable<IFormDataParameters> Parameters { get; set; }
        public IEnumerable<INameValue> Settings { get; set; }
        public string Query { get; set; }
        public string PostData { get; set; }
        public bool IsFormDataChecked { get; set; }
        public bool IsManualChecked { get; set; }
        public bool IsUrlEncodedChecked { get; set; }
    }
    
    public class WebSource : Resource, IDisposable, IResourceSource, IWebSource
    {
        bool _disposed;

        public string Address { get; set; }
        public string DefaultQuery { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AuthenticationType AuthenticationType { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// This must NEVER be persisted - here for JSON transport only!
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// This must NEVER be persisted - here so that we only instantiate once!
        /// </summary>
        public IWebClientWrapper Client { get; set; }
        
        public WebSource()
        {
            ResourceID = Guid.Empty;
            ResourceType = "WebSource";
            AuthenticationType = AuthenticationType.Anonymous;
        }

        public WebSource(XElement xml)
            : base(xml)
        {
            ResourceType = "WebSource";
            AuthenticationType = AuthenticationType.Anonymous;

            var properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Address", string.Empty },
                { "DefaultQuery", string.Empty },
                { "AuthenticationType", string.Empty },
                { "UserName", string.Empty },
                { "Password", string.Empty }
            };

            var conString = xml.AttributeSafe("ConnectionString");
            var connectionString = conString.CanBeDecrypted() ? DpapiWrapper.Decrypt(conString) : conString;
            connectionString = connectionString.UnescapeString();
            ParseProperties(connectionString, properties);
            Address = properties["Address"];
            DefaultQuery = properties["DefaultQuery"];
            UserName = properties["UserName"];
            Password = properties["Password"];

            AuthenticationType = Enum.TryParse(properties["AuthenticationType"], true, out AuthenticationType authType) ? authType : AuthenticationType.Windows;
        }

        public override XElement ToXml()
        {
            var result = base.ToXml();
            var connectionString = string.Join(";",
                $"Address={Address}",
                $"DefaultQuery={DefaultQuery}",
                $"AuthenticationType={AuthenticationType}"
                );

            if (AuthenticationType == AuthenticationType.User)
            {
                connectionString = string.Join(";",
                    connectionString,
                    $"UserName={UserName}",
                    $"Password={Password}"
                    );
            }

            result.Add(
                new XAttribute("ConnectionString", DpapiWrapper.Encrypt(connectionString.EscapeString())),
                new XAttribute("Type", GetType().Name),
                new XElement("TypeOf", ResourceType)
                );

            return result;
        }

        public override bool IsSource => true;
        public override bool IsService => false;

        public override bool IsFolder => false;

        public override bool IsReservedService => false;

        public override bool IsServer => false;

        public override bool IsResourceVersion => false;

        public void DisposeClient()
        {
            if (Client != null)
            {
                Client.Dispose();
                Client = null;
            }
        }

        // This destructor will run only if the Dispose method does not get called. 
        ~WebSource()
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
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    DisposeClient();
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here. 

                // Note disposing has been done.
                _disposed = true;
            }
        }
    }
}
