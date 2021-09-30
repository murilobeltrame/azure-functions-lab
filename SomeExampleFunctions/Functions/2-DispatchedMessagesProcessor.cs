using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SomeExampleFunctions.Models;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System;
using SomeExampleFunctions.Shared;

namespace SomeExampleFunctions.Functions
{
    public class DispatchedMessagesProcessor
    {
        [FunctionName(nameof(DispatchedMessagesProcessor))]
        [FixedDelayRetry(10, "00:00:01")]
        [return: ServiceBus("messagesprocessed", Connection = "BrokerConnectionString")]
        public static async Task<Message> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var functionName = $"2:{nameof(DispatchedMessagesProcessor)}";
            log.LogInformation($"Called '{functionName}'.");

            Failure.AtRateOf(25);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            DispatchingMessage data = JsonConvert.DeserializeObject<DispatchingMessage>(requestBody);

            log.LogInformation($"'{functionName}' received '{requestBody}' as a payload.");

            Failure.AtRateOf(25);

            var result = new ProcessedMessage
            {
                Secret = data.Secret
            };
            var jsonResult = JsonConvert.SerializeObject(result);
            var binaryResult = Encoding.UTF8.GetBytes(jsonResult);

            log.LogInformation($"'{functionName}' returning '{JsonConvert.SerializeObject(result)}'.");

            Failure.AtRateOf(25);

            return new Message
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Body = binaryResult
            };
        }
    }
}
