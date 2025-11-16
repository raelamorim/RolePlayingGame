using Microsoft.AspNetCore.Mvc;
using RolePlayingGame.Application.Dtos.Request;
using RolePlayingGame.Application.Dtos.Response;
using RolePlayingGame.Application.Exceptions;
using RolePlayingGame.Application.Interfaces;

namespace RolePlayingGame.Api.Controllers
{
	[ApiController]
	[Route("api/battles")]
	public class BattleController : ControllerBase
	{
		private readonly ILogger<BattleController> _logger;

		public BattleController(ILogger<BattleController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public async Task<ActionResult<PostBattleResponse>> StartBattle(
			[FromBody] PostBattleRequest request,
			[FromServices] IPostBattleUseCase useCase)
		{
			var response = await useCase.ExecuteAsync(request);
			return Ok(response);
		}
	}
}
