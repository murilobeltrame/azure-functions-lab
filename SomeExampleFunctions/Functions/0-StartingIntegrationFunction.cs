using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SomeExampleFunctions.Models;
using Microsoft.Azure.ServiceBus;
using System;
using Newtonsoft.Json;
using System.Text;

namespace SomeExampleFunctions
{
    public static class StartingIntegrationFunction
    {
        [FunctionName(nameof(StartingIntegrationFunction))]
        [return: ServiceBus("messagesdispatched", Connection = "BrokerConnectionString")]
        public static Message Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var functionName = $"0:{nameof(StartingIntegrationFunction)}";
            log.LogInformation($"Called '{functionName}'.");

            string secret = req?.Query["secret"] ?? "nothing";

            log.LogInformation($"'{functionName}' received '{secret}' as a secret.");

            var content = new DispatchingMessage { Secret = secret };
            var jsonContent = JsonConvert.SerializeObject(content);
            var binaryContent = Encoding.UTF8.GetBytes(jsonContent);

            return new Message
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Body = binaryContent
            };
        }
    }
}
