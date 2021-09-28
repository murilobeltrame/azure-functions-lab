using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SomeExampleFunctions.Models;
using SomeExampleFunctions.Shared;

namespace SomeExampleFunctions.Functions
{
    public static class ProcessedMessagesSubscriberFunction
    {
        [FunctionName(nameof(ProcessedMessagesSubscriberFunction))]
        public static async Task Run(
            [ServiceBusTrigger(
                "messagesprocessed",
                "MessagesProcessedSubscription",
                Connection = "BrokerConnectionString"
            )] string message,
            string messageId,
            string correlationId,
            ILogger log)
        {
            var functionName = $"3:{nameof(ProcessedMessagesSubscriberFunction)}";
            log.LogInformation($"'{functionName}' triggered with message: {message}");

            var cosmosClient = new DatabaseClient();
            var integrationData = new Integration
            {
                JobName = nameof(ProcessedMessagesSubscriberFunction),
                MessageId = messageId,
                CorrelationId = correlationId
            };

            try
            {
                await cosmosClient.Add(integrationData);

                var blobClient = new BlobClient();
                var blobReference = await blobClient.WriteFile($"{Guid.NewGuid()}.json", message);

                integrationData.MessageBodyFilePath = blobReference;
                await cosmosClient.Upsert(integrationData);

                integrationData.Status = IntegrationStatus.SUCCESS;
                await cosmosClient.Upsert(integrationData);
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
