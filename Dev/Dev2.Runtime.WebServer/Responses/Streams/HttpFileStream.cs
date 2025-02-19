#pragma warning disable
/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Dev2.Runtime.WebServer.Responses.Streams
{
    public class HttpFileStream : HttpPushContentStream
    {
        readonly Func<Stream> _openInputStream;
        public HttpFileStream(Func<Stream> openInputStream, HttpResponseMessage response, MediaTypeHeaderValue contentType)
            : this(openInputStream, response, contentType, DefaultChunkSize)
        {
        }

        public HttpFileStream(Func<Stream> openInputStream, HttpResponseMessage response, MediaTypeHeaderValue contentType, int chunkSize)
            : base(response, contentType, chunkSize)
        {
            VerifyArgument.IsNotNull("openInputStream", openInputStream);
            _openInputStream = openInputStream;
        }

        protected override Stream OpenInputStream() => _openInputStream();
    }
}
