using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Avro
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string connectionString = "[connectionString]";
            string containerName = "synergi-staging";

            CleanerClient cleaner = new CleanerClient(connectionString, containerName, log);
            List<string> columns = new List<string>()
            {
                "ACTION_NO",
                "ACTION_COMMENT"
            };
            cleaner.RunCleaning("Action.avro", columns);

            return req.CreateResponse(HttpStatusCode.OK, "OK");
        }
    }
}
