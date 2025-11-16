using RolePlayingGame.Api.Configuration;
using RolePlayingGame.Application.Configuration;
using RolePlayingGame.Infrastructure.Configuration;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;

namespace RolePlayingGame.Api
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		private readonly string _environment;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			_environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// Controllers
			services.AddControllers();

			// Swagger
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			// OpenTelemetry / Observability
			services.ConfigureOpenTelemetry(Configuration);

			// Application layer
			ApplicationConfiguration.ConfigureServices(services);

			// Infrastructure layer
			InfrastructureConfiguration.ConfigureServices(services, Configuration);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// Logging
			LoggingConfiguration.ConfigureLogger(Configuration, _environment);

			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseMiddleware<GlobalExceptionMiddleware>();

			// Only use HTTPS redirection if the server actually exposes an HTTPS address.
			// This prevents "Failed to determine the https port for redirect." in containers
			// where HTTPS is not configured.
			var addressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
			if (addressesFeature != null && addressesFeature.Addresses.Any(a => a.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
			{
				app.UseHttpsRedirection();
			}

			app.UseRouting();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
