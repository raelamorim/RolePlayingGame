using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace RolePlayingGame.Api.Configuration
{
	public static class ObservabilityConfiguration
	{
		private const string ServiceName = "RolePlayingGame";
		private const string ServiceVersion = "1.0.0";

		public static IServiceCollection ConfigureOpenTelemetry(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			var otelEndpoint = configuration.GetValue<string>("OpenTelemetry:Endpoint") ?? "http://localhost:4317";

			var resource = ResourceBuilder
				.CreateDefault()
				.AddService(
					serviceName: ServiceName,
					serviceVersion: ServiceVersion);

			// Configure Tracing
			services.AddOpenTelemetry()
				.WithTracing(tracing =>
				{
					tracing
						.SetResourceBuilder(resource)
						.AddAspNetCoreInstrumentation()
						.AddHttpClientInstrumentation()
						.AddOtlpExporter(opts => opts.Endpoint = new Uri(otelEndpoint));
				})
				.WithMetrics(metrics =>
				{
					metrics
						.SetResourceBuilder(resource)
						.AddAspNetCoreInstrumentation()
						.AddHttpClientInstrumentation()
						.AddOtlpExporter(opts => opts.Endpoint = new Uri(otelEndpoint));
				});

			// Configure Logging with OpenTelemetry
			services.AddLogging(logging =>
			{
				logging.AddOpenTelemetry(opts =>
				{
					opts.SetResourceBuilder(resource)
						.AddOtlpExporter(otlpOpts => otlpOpts.Endpoint = new Uri(otelEndpoint));
				});
			});

			return services;
		}
	}
}
