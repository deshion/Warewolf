/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
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
using Dev2.Common.Interfaces.Infrastructure.SharedModels;
using Dev2.Runtime.ServiceModel.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Warewolf.Security.Encryption;
using Warewolf.Sharepoint;

namespace Dev2.Data.ServiceModel
{
    public class SharepointSource : Resource, ISharepointSource, IResourceSource
    {
        private readonly ISharepointHelperFactory _sharepointHelperFactory;

        public string Server { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AuthenticationType AuthenticationType { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public SharepointSource()
            :this(new SharepointHelperFactory())
        {

        }

        public SharepointSource(ISharepointHelperFactory sharepointHelperFactory)
        {
            _sharepointHelperFactory = sharepointHelperFactory;
            ResourceID = Guid.Empty;
            ResourceType = "SharepointServerSource";
            AuthenticationType = AuthenticationType.Windows;
        }

        public SharepointSource(XElement xml)
            : this(xml, new SharepointHelperFactory())
        {

        }

        public SharepointSource(XElement xml, ISharepointHelperFactory sharepointHelperFactory)
            : base(xml)
        {
            _sharepointHelperFactory = sharepointHelperFactory;
            ResourceType = "SharepointServerSource";
            AuthenticationType = AuthenticationType.Windows;

            var properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Server", string.Empty },
                { "AuthenticationType", string.Empty },
                { "UserName", string.Empty },
                { "Password", string.Empty }
            };

            var conString = xml.AttributeSafe("ConnectionString");
            var connectionString = conString.CanBeDecrypted() ? DpapiWrapper.Decrypt(conString) : conString;
            ParseProperties(connectionString, properties);
            Server = properties["Server"];
            UserName = properties["UserName"];
            Password = properties["Password"];
            var isSharepointSourceValue = xml.AttributeSafe("IsSharepointOnline");
            if (bool.TryParse(isSharepointSourceValue, out bool isSharepointSource))
            {
                IsSharepointOnline = isSharepointSource;
            }
            AuthenticationType = Enum.TryParse(properties["AuthenticationType"], true, out AuthenticationType authType) ? authType : AuthenticationType.Windows;
        }

        public override XElement ToXml()
        {
            var result = base.ToXml();
            var connectionString = string.Join(";",
                $"Server={Server}",
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
                new XAttribute("ConnectionString", DpapiWrapper.Encrypt(connectionString)),
                new XAttribute("IsSharepointOnline", IsSharepointOnline),
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

        public List<ISharepointListTo> LoadLists()
        {
            var sharepointHelper = CreateSharepointHelper();
            return sharepointHelper.LoadLists();
        }

        public List<ISharepointFieldTo> LoadFieldsForList(string listName) => LoadFieldsForList(listName, false);

        public List<ISharepointFieldTo> LoadFieldsForList(string listName, bool editableFieldsOnly)
        {
            var sharepointHelper = CreateSharepointHelper();
            return sharepointHelper.LoadFieldsForList(listName, editableFieldsOnly);
        }

        public virtual ISharepointHelper CreateSharepointHelper()
        {
            string userName = null;
            string password = null;

            if (AuthenticationType == AuthenticationType.User)
            {
                userName = UserName;
                password = Password;
            }
            var sharepointHelper = _sharepointHelperFactory.New(Server, userName, password, IsSharepointOnline);
            return sharepointHelper;
        }

        public string TestConnection()
        {
            var helper = CreateSharepointHelper();
            var testConnection = helper.TestConnection(out bool isSharepointOnline);
            IsSharepointOnline = isSharepointOnline;
            return testConnection;
        }

        public bool IsSharepointOnline { get; set; }
    }
}