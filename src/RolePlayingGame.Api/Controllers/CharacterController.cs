using Microsoft.AspNetCore.Mvc;
using RolePlayingGame.Application.Dtos.Request;
using RolePlayingGame.Application.Dtos.Response;
using RolePlayingGame.Application.Interfaces;

namespace RolePlayingGame.Api.Controllers
{
	[ApiController]
	[Route("api/characters")]
	public class CharacterController : ControllerBase
	{
		private readonly ILogger<CharacterController> _logger;

		public CharacterController(ILogger<CharacterController> logger)
		{
			_logger = logger;
		}

		// POST api/characters
		[HttpPost]
		public async Task<ActionResult<PostCharacterResponse>> CreateCharacterAsync(
			[FromBody] PostCharacterRequest request,
			[FromServices] IPostCharacterUseCase characterService
		)
		{
			if (!ModelState.IsValid)
			{
				_logger.LogWarning("Invalid character creation request: {@Request}", request);
				return BadRequest(ModelState);
			}

			var character = await characterService.ExecuteAsync(request);
			return Ok(character);
		}

		// GET api/characters
		[HttpGet]
		public async Task<ActionResult<IEnumerable<GetCharacterListResponse>>> GetCharactersAsync(
			[FromServices] IGetCharacterListUseCase useCase
		)
		{
			var result = await useCase.ExecuteAsync();
			return Ok(result);
		}

		// GET api/characters/{id}
		[HttpGet("{id:guid}")]
		public async Task<ActionResult<GetCharacterDetailResponse>> GetCharacterAsync(
			Guid id,
			[FromServices] IGetCharacterDetailUseCase useCase
		)
		{
			var result = await useCase.ExecuteAsync(id);
			return Ok(result);
		}
	}
}
