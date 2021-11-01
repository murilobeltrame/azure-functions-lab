using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SomeExampleFunctions.Shared;
using System;

[assembly: FunctionsStartup(typeof(SomeExampleFunctions.Startup))]
namespace SomeExampleFunctions
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddHttpClient("processorApi", client =>
			{
				client.BaseAddress = new Uri(Configuration.ValueOf("MessageProcessorServiceUrl"));
			});

			builder.Services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.ClearProvidersExceptFunctionProviders();
				loggingBuilder.AddSerilog(SerilogConfiguration.Configure().CreateLogger());
			});
		}
	}
}