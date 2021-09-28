using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SomeExampleFunctions.Models;

namespace SomeExampleFunctions
{
    public static class StartingIntegrationFunction
    {
        [FunctionName("StartingIntegrationFunction")]
        [return: ServiceBus("messagesdispatched", Connection = "BrokerConnectionString")]
        public static DispatchingMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Called 'StartingIntegrationFunction'.");

            string secret = req?.Query["secret"] ?? "nothing";

            log.LogInformation($"'StartingIntegrationFunction' received '{secret}' as a secret.");

            return new DispatchingMessage { Secret = secret };
        }
    }
}
