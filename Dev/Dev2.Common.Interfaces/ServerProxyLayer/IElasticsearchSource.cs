﻿/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2021 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Dev2.Common.Interfaces.Data;
using Dev2.Runtime.ServiceModel.Data;

namespace Dev2.Common.Interfaces.ServerProxyLayer
{
    public interface IElasticsearchSource : IResource
    {
        string HostName { get; set; }
        string Password { get; set; }
        string Username { get; set; }
        string Port { get; set; }
        AuthenticationType AuthenticationType { get; set; }
        string SearchIndex { get; set; }
        void Dispose();
    }
}