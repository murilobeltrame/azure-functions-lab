using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace SomeExampleFunctions
{
    public static class DispatchedMessagesPublisherFunction
    {
        [FunctionName("DispatchedMessagesPublisherFunction")]
        public static void Run(
            [ServiceBusTrigger(
                "messagesdispatched",
                "MessagesDispatchedSubscription",
                Connection = "BrokerConnectionString"
            )] string message,
            ILogger log)
        {
            log.LogInformation($"'DispatchedMessagesPublisherFunction' triggered with message: {message}");
        }
    }
}
