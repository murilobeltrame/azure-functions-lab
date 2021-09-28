# azure-functions-lab

A Lab to tryout workflows with C# Azure Functions using HTTP and Azure Service Bus Triggers

## Implementation

![Implementation](docs/images/Implementation.png)

The lab consisting in:
- A `0-StartingIntegrationFunction` function exposing a HTTP API to start the process flow;
- `0-StartingIntegrationFunction` function publish a message in the `messagedispatched` Azure Service Bus Topic;
- `1-DispatchedMessagesPublisherFunction` function listen the topic via `MessageDispatchedSubscription` and POST data to a HTTP API exposed by `2-DispatchedMessagesProcessor` function;
- `2-DispatchedMessagesProcessor` function publish another processed message into `messagesprocessed` Azure Service Bus Topic;
- `3-ProcessedMessagesSubscriberFunction` listen the topic via `MessagesProcessedSubscription`;

All the functions has instructions (`Failure.AtRateOf(xx);`) to fail at some "rate" to simulate failures and prove process resiliency via retries. <br />
Functions writes Message's metadata into CosmosDb with MongoDb API and Message's body into Azure Storage Account to keep track of the process state
