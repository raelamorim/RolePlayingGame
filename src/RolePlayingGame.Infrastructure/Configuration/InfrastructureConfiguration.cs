using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RolePlayingGame.Domain.Gateways;
using RolePlayingGame.Infrastructure.Databases.Repositories;
using RolePlayingGame.Insfrastructure.Databases.Context;

namespace RolePlayingGame.Infrastructure.Configuration
{
	public class InfrastructureConfiguration
	{
		public static void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
		{
			services.AddDbContext<RolePlayingGameDbContext>(options =>
			{
				options.UseInMemoryDatabase("GameDb");
			});

			services.AddScoped<ICharacterGateway, CharacterRepository>();
		}
	}
}
