﻿/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/
using Dev2.Common;
using Dev2.Common.Interfaces.Core;
using Dev2.Communication;
using Dev2.Data.ServiceModel;
using Dev2.DynamicServices;
using Dev2.Runtime.Diagnostics;
using Dev2.Runtime.ServiceModel;
using Dev2.Workspaces;
using System;
using System.Collections.Generic;
using System.Text;
using Dev2.Common.Interfaces.Enums;

namespace Dev2.Runtime.ESB.Management.Services
{
    public class TestRedisSource : IEsbManagementEndpoint
    {
        public Guid GetResourceID(Dictionary<string, StringBuilder> requestArgs) => Guid.Empty;

        public AuthorizationContext GetAuthorizationContextForService() => AuthorizationContext.Contribute;

        public StringBuilder Execute(Dictionary<string, StringBuilder> values, IWorkspace theWorkspace)
        {
            var msg = new ExecuteMessage();
            var serializer = new Dev2JsonSerializer();
            try
            {
                Dev2Logger.Info("Test Redis Source", GlobalConstants.WarewolfInfo);
                msg.HasError = false;
                values.TryGetValue(Warewolf.Service.TestRedisSource.RedisSource, out StringBuilder resourceDefinition);

                var redisServiceSourceDefinition = serializer.Deserialize<RedisSourceDefinition>(resourceDefinition);
                var con = new RedisSources();
                using (var redisSource = new RedisSource
                {
                    HostName = redisServiceSourceDefinition.HostName,
                    Port = redisServiceSourceDefinition.Port,
                    AuthenticationType = redisServiceSourceDefinition.AuthenticationType,
                    Password = redisServiceSourceDefinition.Password
                })
                {
                    var result = con.Test(redisSource);
                    msg.HasError = false;
                    msg.Message = new StringBuilder(result.IsValid ? serializer.Serialize(result.Result) : result.ErrorMessage);
                    msg.HasError = !result.IsValid;
                }
            }
            catch (Exception err)
            {
                msg.HasError = true;
                msg.Message = new StringBuilder(err.Message);
                Dev2Logger.Error(err, GlobalConstants.WarewolfError);

            }

            return serializer.SerializeToBuilder(msg);
        }

        public DynamicService CreateServiceEntry() => EsbManagementServiceEntry.CreateESBManagementServiceEntry(HandlesType(), "<DataList><Roles ColumnIODirection=\"Input\"/><RedisSource ColumnIODirection=\"Input\"/><WorkspaceID ColumnIODirection=\"Input\"/><Dev2System.ManagmentServicePayload ColumnIODirection=\"Both\"></Dev2System.ManagmentServicePayload></DataList>");

        public string HandlesType() => nameof(Warewolf.Service.TestRedisSource);
    }
}
