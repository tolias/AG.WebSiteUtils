namespace AG.WebSiteUtils
{
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// Extension methods for HTTP Request.
    /// <remarks>
    /// See the HTTP 1.1 specification http://www.w3.org/Protocols/rfc2616/rfc2616.html
    /// for details of implementation decisions.
    /// </remarks>
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Dump the raw http request to a string. 
        /// </summary>
        /// <param name="request">The <see cref="HttpRequest"/> that should be dumped.       </param>
        /// <returns>The raw HTTP request.</returns>
        public static string ToRaw(this HttpRequestBase request)
        {
            StringWriter writer = new StringWriter();

            WriteStartLine(request, writer);
            WriteHeaders(request, writer);
            WriteBody(request, writer);

            return writer.ToString();
        }

        private static void WriteStartLine(HttpRequestBase request, StringWriter writer)
        {
            const string SPACE = " ";

            writer.Write(request.HttpMethod);
            writer.Write(SPACE + request.Url);
            writer.WriteLine(SPACE + request.ServerVariables["SERVER_PROTOCOL"]);
        }

        private static void WriteStartLine(HttpRequestMessage httpRequestMessage, StringWriter writer)
        {
            const string SPACE = " ";

            writer.Write(httpRequestMessage.Method.Method);
            writer.WriteLine(SPACE + httpRequestMessage.RequestUri);
            //writer.WriteLine(SPACE + httpRequestMessage.ServerVariables["SERVER_PROTOCOL"]);
        }

        private static void WriteHeaders(HttpRequestBase request, StringWriter writer)
        {
            var headers = request.Headers;
            foreach (string key in headers.AllKeys)
            {
                writer.WriteLine(string.Format("{0}: {1}", key, headers[key]));
            }

            writer.WriteLine();
        }

        private static void WriteHeaders(HttpRequestHeaders httpRequestHeaders, StringWriter writer)
        {
            var rawHeaders = httpRequestHeaders.ToString();
            writer.Write(rawHeaders);
            //foreach (var headersByName in httpRequestHeaders)
            //{
            //    foreach (var headerValue in headersByName.Value)
            //    {
            //        writer.WriteLine(string.Format("{0}: {1}", headersByName.Key, headerValue));
            //    }
            //}
            writer.WriteLine();
        }

        private static void WriteBody(HttpRequestBase request, StringWriter writer)
        {
            StreamReader reader = new StreamReader(request.InputStream);

            try
            {
                string body = reader.ReadToEnd();
                writer.WriteLine(body);
            }
            finally
            {
                reader.BaseStream.Position = 0;
            }
        }

        private static async Task WriteBody(HttpContent httpContent, StringWriter writer)
        {
            var body = await httpContent.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(body))
            {
                writer.WriteLine(body);
            }
        }

        public static async Task<string> ToRaw(this HttpRequestMessage httpRequestMessage)
        {
            StringWriter writer = new StringWriter();

            WriteStartLine(httpRequestMessage, writer);
            WriteHeaders(httpRequestMessage.Headers, writer);
            await WriteBody(httpRequestMessage.Content, writer);

            return writer.ToString();
        }
    }
}