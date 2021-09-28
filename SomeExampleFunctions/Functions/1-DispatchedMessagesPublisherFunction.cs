using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SomeExampleFunctions.Models;
using SomeExampleFunctions.Shared;

namespace SomeExampleFunctions
{
    public class DispatchedMessagesPublisherFunction
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public DispatchedMessagesPublisherFunction(
            IConfiguration configuration,
            IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("processorApi");
            _configuration = configuration;
        }

        [FunctionName(nameof(DispatchedMessagesPublisherFunction))]
        public async Task Run(
            [ServiceBusTrigger(
                "messagesdispatched",
                "MessagesDispatchedSubscription",
                Connection = "BrokerConnectionString"
            )] string message,
            string messageId,
            string correlationId,
            ILogger log)
        {
            var functionName = $"1:{nameof(DispatchedMessagesPublisherFunction)}";
            log.LogInformation($"'{functionName}' triggered with message: {message}");
            
            var cosmosClient = new DatabaseClient();
            var integrationData = new Integration
            {
                JobName = nameof(DispatchedMessagesPublisherFunction),
                MessageId = messageId,
                CorrelationId = correlationId
            };

            try
            {
                await cosmosClient.Add(integrationData);

                Failure.AtRateOf(25);

                var blobClient = new BlobClient();
                var blobReference = await blobClient.WriteFile($"{Guid.NewGuid()}.json", message);

                integrationData.MessageBodyFilePath = blobReference;
                await cosmosClient.Upsert(integrationData);

                Failure.AtRateOf(25);

                var path = "DispatchedMessagesProcessor";
                var payload = new StringContent(message, Encoding.UTF8, "application/json");
                var result = await _client.PostAsync(path, payload);
                if (result.IsSuccessStatusCode) {
                    integrationData.Status = IntegrationStatus.SUCCESS;
                    await cosmosClient.Upsert(integrationData);
                }
            }
            catch (Exception ex)
            {
                integrationData.Status = IntegrationStatus.FAILURE;
                integrationData.ExceptionMessage = ex.Message;
                await cosmosClient.Upsert(integrationData);
                throw ex;
            }
        }
    }
}
