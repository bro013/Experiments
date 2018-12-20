using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using OmniaTransferLogDev.Models;

namespace OmniaTransferLogDev.Logging
{
    public class EventGrid
    {
        public LogItem Item { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }
        public EventGrid(LogItem item, string key, string url)
        {
            Item = item;
            Key = key;
            Url = url;
        }

        /// <summary>
        /// Posting log data using HTTP request. Prompting Key Vault for authroization key
        /// </summary>
        public void PostEventGrid()
        {
                string json = GetJsonString();
                string url = Url;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("aeg-sas-key", Key);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                Task<HttpResponseMessage> response = client.PostAsync(new Uri(url), httpContent);

        }

        /// <summary>
        /// Creates a formatted Json string for Event Grid
        /// </summary>
        /// <returns></returns>
        public string GetJsonString()
        {
            var Data = new Dictionary<string, object>
            {
                {"dataFlowName", Item.DataFlowName },
                {"source", Item.Source},
                {"sink", Item.Sink },
                {"type", Item.Type },
                {"errorMessage", Item.ErrorMessage },
                {"mailRecipient",  Item.MailRecipient}
            };

            var Event = new Dictionary<string, object>
            {
                { "id", Item.RunId },
                { "subject", Item.Subject },
                { "eventType", Item.Type },
                { "eventTime", DateTime.UtcNow },
                { "dataVersion", "1.0" },
                { "data",Data }
            };

            return "[" + JsonConvert.SerializeObject(Event) + "]";
        }
    }
}
