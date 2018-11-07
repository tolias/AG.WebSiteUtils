using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AG.WebSiteUtils
{
    public class GlobalRequestsHandlerLogger : DelegatingHandler
    {
        private RequestsLogger _requestsLogger;

        public GlobalRequestsHandlerLogger(RequestsLogger requestsLogger)
        {
            _requestsLogger = requestsLogger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
                                                HttpRequestMessage request,
                                                   CancellationToken token)
        {
            //HttpMessageContent requestContent = new HttpMessageContent(request);
            //await requestContent.LoadIntoBufferAsync();

            //string rawRequest;

            //using (var stream = new MemoryStream())
            //{
            //    var context = (HttpContextBase)request.Properties["MS_HttpContext"];
            //    context.Request.InputStream.Seek(0, SeekOrigin.Begin);
            //    context.Request.InputStream.CopyTo(stream);
            //    rawRequest = Encoding.UTF8.GetString(stream.ToArray());
            //}

            //var requestStream = await requestContent.ReadAsStreamAsync();
            //requestStream.Seek(0, System.IO.SeekOrigin.Begin);

            //var reader = new StreamReader(requestStream);
            //var result = reader.ReadToEnd();
            //requestStream.Seek(0, SeekOrigin.Begin);

            //string rawRequest = await requestContent.ReadAsStringAsync();
            //requestContent.ReadAsStreamAsync

            await _requestsLogger.LogRequest(request);

            return await base.SendAsync(request, token);
        }
    }
}
