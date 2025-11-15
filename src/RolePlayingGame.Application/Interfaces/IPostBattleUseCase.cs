using RolePlayingGame.Application.Dtos.Request;
using RolePlayingGame.Application.Dtos.Response;

namespace RolePlayingGame.Application.Interfaces
{
    public interface IPostBattleUseCase
    {
		Task<PostBattleResponse> ExecuteAsync(PostBattleRequest request);
	}
}
