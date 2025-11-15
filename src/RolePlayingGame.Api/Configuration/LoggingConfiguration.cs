using Serilog;
using Serilog.Filters;
using Serilog.Formatting.Elasticsearch;

namespace RolePlayingGame.Infrastructure.Configuration
{
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
				.Enrich.WithProperty("Environment", environment)
#if DEBUG
				.WriteTo.Console();
#else
				.WriteTo.Console(new ElasticsearchJsonFormatter());
#endif

			Log.Logger = loggerConfiguration.CreateLogger();
		}
	}
}
