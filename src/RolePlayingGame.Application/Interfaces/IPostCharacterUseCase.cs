using RolePlayingGame.Application.Dtos.Request;
using RolePlayingGame.Application.Dtos.Response;

namespace RolePlayingGame.Application.Interfaces
{
    public interface IPostCharacterUseCase
    {
		Task<PostCharacterResponse> ExecuteAsync(PostCharacterRequest request);
    }
}
