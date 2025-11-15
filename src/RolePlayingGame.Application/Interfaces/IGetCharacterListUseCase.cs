using RolePlayingGame.Application.Dtos.Response;

namespace RolePlayingGame.Application.Interfaces
{
    public interface IGetCharacterListUseCase
    {
		Task<IEnumerable<GetCharacterListResponse>> ExecuteAsync();
	}
}
