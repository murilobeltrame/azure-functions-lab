using Serilog;
using Serilog.Events;
using System;

namespace SomeExampleFunctions.Shared
{
	public static class SerilogConfiguration
	{
		public static LoggerConfiguration Configure()
		{
			LoggerConfiguration configuration = GetLoggerConfiguration();
			SetLevelOverrides(configuration);
			SetEnrichers(configuration);
			SetConsoleSink(configuration);
			SetDataDogLogsSink(configuration);
			return configuration;
		}//func

		private static LoggerConfiguration GetLoggerConfiguration()
		{
			LoggerConfiguration config = new LoggerConfiguration();
			return config;
		}//func

		private static void SetLevelOverrides(LoggerConfiguration configuration)
		{
			configuration
				.MinimumLevel.Override("Default", LogEventLevel.Information)
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information);
		}//func

		private static void SetEnrichers(LoggerConfiguration configuration)
		{
			configuration
				.Enrich.FromLogContext()
				.Enrich.WithThreadId()
				.Enrich.WithThreadName();
		}//func

		private static void SetConsoleSink(LoggerConfiguration configuration)
		{
			configuration.WriteTo.Console(outputTemplate: Configuration.ValueOf("Serilog.Console.OutputTemplate"));
		}//func

		private static void SetDataDogLogsSink(LoggerConfiguration configuration)
		{
			configuration
				.WriteTo.DatadogLogs(
					apiKey: Configuration.ValueOf("Serilog.DataDogLogs.ApiKey"),
					source: Configuration.ValueOf("Serilog.DataDogLogs.Source"),
					service: Configuration.ValueOf("Serilog.DataDogLogs.Service"),
					host: Configuration.ValueOf("Serilog.DataDogLogs.Host"),
					tags: new[] { $"env:{Configuration.ValueOf("Serilog.DataDogLogs.Env")}" },
					logLevel: LogEventLevel.Debug,
					batchSizeLimit: Convert.ToInt32(Configuration.ValueOf("Serilog.DataDogLogs.BatchSizeLimit")),
					queueLimit: Convert.ToInt32(Configuration.ValueOf("Serilog.DataDogLogs.QueueLimit"))
				);
		}//func
	}
}
