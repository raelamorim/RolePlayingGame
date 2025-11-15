using Microsoft.Extensions.DependencyInjection;
using RolePlayingGame.Application.Interfaces;
using RolePlayingGame.Application.UseCase;

namespace RolePlayingGame.Application.Configuration
{
    public static class ApplicationConfiguration
    {
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<IPostCharacterUseCase, PostCharacterUseCase>();
			services.AddScoped<IGetCharacterListUseCase, GetCharacterListUseCase>();
			services.AddScoped<IGetCharacterDetailUseCase, GetCharacterDetailUseCase>();
		}
	}
}
