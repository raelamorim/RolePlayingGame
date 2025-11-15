using RolePlayingGame.Application.Dtos.Response;

namespace RolePlayingGame.Application.Interfaces
{
    public interface IGetCharacterDetailUseCase
    {
		Task<GetCharacterDetailResponse?> ExecuteAsync(Guid id);
	}
}
