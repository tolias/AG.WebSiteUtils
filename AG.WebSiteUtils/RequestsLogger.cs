using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AG.PathStringOperations;
using System.Net.Http;

namespace AG.WebSiteUtils
{
    public class RequestsLogger
    {
        private string _logDir;

        public RequestsLogger(string logDir)
        {
            _logDir = logDir;
        }

        private string GetRequestLogFileName(HttpRequestBase request)
        {
            return GetRequestLogFileName(request.UserHostAddress);
        }

        private string GetRequestLogFileName(string ip)
        {
            var fileName = $"{ip}";
            fileName = ExtendedPath.GetRightFileNameFromString(fileName);
            fileName += ".txt";
            var fullFileName = Path.Combine(_logDir, fileName);
            return fullFileName;
        }

        public void LogRequest(Controller controller)
        {
            var request = controller.Request;

            string requestFileName = GetRequestLogFileName(request);

            var rawRequest = request.ToRaw();
            AppendToFile(requestFileName, rawRequest);
        }

        public async Task LogRequest(HttpRequestMessage httpRequestMessage)
        {
            var rawRequest = await httpRequestMessage.ToRaw();
            LogRequest(rawRequest);
        }

        public void LogRequest(string rawRequest)
        {
            var clientIp = HttpContext.Current.Request.UserHostAddress;

            string requestFileName = GetRequestLogFileName(clientIp);
            
            AppendToFile(requestFileName, rawRequest);
        }

        private void AppendToFile(string fileName, string content)
        {
            var timeString = DateTime.UtcNow.ToString(@"yy\.MM\.dd HH\:mm\:ss\,fff");

            FileDirectoryManager.CreateDirectoryIfItDoesntExistForFile(fileName, () =>
            {
                using (var sw = new StreamWriter(fileName, true))
                {
                    sw.Write(timeString);
                    sw.Write(' ');
                    sw.WriteLine(content);
                }
            });
        }
    }
}
