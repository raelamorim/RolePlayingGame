using RolePlayingGame.Api.Configuration;
using RolePlayingGame.Application.Configuration;
using RolePlayingGame.Infrastructure.Configuration;

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

			app.UseHttpsRedirection();

			app.UseRouting();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
