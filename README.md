# Azure Functions Lab

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
Functions triggered by HTTP handles retries via attribute decoration (`[FixedDelayRetry(xx, "hh:mm:ss")]`) while Service Bus triggered Functions retries automatically whenever the function fails based on `MaxDeliveryCount` of the Service Bus Topic`s Subscription.<br />
Functions writes Message's metadata into CosmosDb with MongoDb API and Message's body into Azure Storage Account to keep track of the process state, these data are expose by a REST API maded of HTTP triggered Azure Functions.

## Local Configuration
After running Terraform infrastructure automation, you should create and fill some configuration values in local.settings.json file at project's root.
This file should not be uploaded to the repository as it contains sensitive data.
```json
{
	"IsEncrypted": false,
	"Values": {
		"MessageProcessorServiceUrl": "http://localhost:7071/api/",
		"FUNCTIONS_WORKER_RUNTIME": "dotnet",
		"AzureWebJobsStorage": "UseDevelopmentStorage=true",
		"BrokerConnectionString": "<run: terraform output BrokerConnectionString>",
		"DatabaseConnectionString": "<run: terraform output DatabaseConnectionString>",
		"StorageConnectionString": "<run: terraform output StorageConnectionString>",
		"DatabaseCollection": "integrations",
		"DatabaseName": "dbintegrationlab",
		"StorageContainerName": "messages",
		"Serilog.DataDogLogs.ApiKey": "dummy",
		"Serilog.DataDogLogs.Source": "azure-functions-lab",
		"Serilog.DataDogLogs.Service": "some-example-functions",
		"Serilog.DataDogLogs.Env": "azfn-lab",
		"Serilog.DataDogLogs.Host": "azure",
		"Serilog.DataDogLogs.BatchSizeLimit": 5000,
		"Serilog.DataDogLogs.QueueLimit": 500000
	}
}
```

## Running

```sh
# '~' is used to represent the project`s root folder
# create adjacent infrastructure
$ cd ~/environment/terraform
$ az login
$ terraform init
$ terraform apply
$ terraform output <property> (for sensitive data)

# running the project locally
$ cd ~/SomeExampleFunctions
$ func start --verbose
```

## Dependencies

- [.Net 5.0](https://dotnet.microsoft.com/download)
- [Azure Functions Core Tools 3.0](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#install-the-azure-functions-core-tools)
- [Azure CLI 2.28](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Terraform 1.0](https://www.terraform.io/downloads.html)
- [Free Azure Account](https://azure.microsoft.com/free/) *(services used in this lab aren`t free)*
