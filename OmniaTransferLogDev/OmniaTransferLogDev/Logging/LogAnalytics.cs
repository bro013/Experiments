using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OmniaTransferLogDev.Models;

namespace OmniaTransferLogDev.Logging
{
    public class LogAnalytics
    {
        public LogItem Item { get; set; }
        public string WorkspaceId { get; set; }
        public string SharedKey { get; set; }
        public string LogName { get; set; }

        public LogAnalytics(LogItem item, string sharedKey, string workspaceid, string logName)
        {
            Item = item;
            SharedKey = sharedKey;
            WorkspaceId = workspaceid;
            LogName = logName;
        }

        /// <summary>
        /// Gets the API signature
        /// </summary>
        /// <param name="json"></param>
        /// <param name="datestring"></param>
        /// <returns></returns>
        private string GetSignature(string json, string datestring)
        {
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            string stringToHash = "POST\n" + jsonBytes.Length + "\napplication/json\n" + "x-ms-date:" + datestring + "\n/api/logs";
            string hashedString = BuildSignature(stringToHash, SharedKey);
            string signature = "SharedKey " + WorkspaceId + ":" + hashedString;
            return signature;
        }

        /// <summary>
        /// Build the API signature
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        private string BuildSignature(string message, string secret)
        {
            var encoding = new ASCIIEncoding();
            byte[] keyByte = Convert.FromBase64String(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hash = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Send a request to the POST API endpoint
        /// </summary>
        /// <param name="json"></param>
        public void PostData()
        {

            var json = JsonConvert.SerializeObject(Item);
            string date = DateTime.UtcNow.ToString("r");
            string url = "https://" + WorkspaceId + ".ods.opinsights.azure.com/api/logs?api-version=2016-04-01";
            string signature = GetSignature(json, date);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Log-Type", LogName);
            client.DefaultRequestHeaders.Add("Authorization", signature);
            client.DefaultRequestHeaders.Add("x-ms-date", date);
            //client.DefaultRequestHeaders.Add("time-generated-field", TimeStampField);

            HttpContent httpContent = new StringContent(json, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            Task<HttpResponseMessage> response = client.PostAsync(new Uri(url), httpContent);

        }

    }

}
