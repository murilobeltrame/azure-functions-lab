//using System;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.Extensions.Logging;

//namespace SomeExampleFunctions.Functions
//{
//    public static class ProcessedMessagesSubscriberFunction
//    {
//        [FunctionName("ProcessedMessagesSubscriberFunction")]
//        public static void Run([ServiceBusTrigger("MessagesProcessed", "MessagesProcessedSubscription", Connection = "BrokerConnectionString")] string mySbMsg, ILogger log)
//        {
//            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
//        }
//    }
//}
