using Serilog;
using Serilog.Formatting.Elasticsearch;
using System.Diagnostics.CodeAnalysis;

namespace RolePlayingGame.Api.Configuration
{
	[ExcludeFromCodeCoverage]
	public static class LoggingConfiguration
	{
		public static void ConfigureLogger(IConfiguration configuration, string environment)
		{
			var loggerConfiguration = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.Enrich.FromLogContext()
				.Enrich.WithEnvironmentName()
				.Enrich.WithMachineName()
				.Enrich.WithThreadId()
				.Enrich.WithCorrelationId()
				.Enrich.WithProperty("Application", "RolePlayingGameApi")
				.Enrich.WithProperty("Environment", environment);

			if (environment == "Development")
				loggerConfiguration.WriteTo.Console();
			else
				loggerConfiguration.WriteTo.Console(new ElasticsearchJsonFormatter());

			Log.Logger = loggerConfiguration.CreateLogger();
		}
	}
}
