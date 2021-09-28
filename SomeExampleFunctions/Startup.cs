using System;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SomeExampleFunctions.Shared;

[assembly: FunctionsStartup(typeof(SomeExampleFunctions.Startup))]
namespace SomeExampleFunctions
{
    public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient("processorApi", client => {
                client.BaseAddress = new Uri(Configuration.ValueOf("MessageProcessorServiceUrl"));
            }); 
        }
    }
}