
using Microsoft.Extensions.Logging;
using RolePlayingGame.Application.Dtos.Request;
using RolePlayingGame.Application.Dtos.Response;
using RolePlayingGame.Application.Interfaces;
using RolePlayingGame.Application.Mappers;
using RolePlayingGame.Domain.Gateways;
using System.Text.Json;

namespace RolePlayingGame.Application.UseCase
{
	public class PostCharacterUseCase : IPostCharacterUseCase
	{
		private readonly ILogger<PostCharacterUseCase> _logger;
		private readonly ICharacterGateway _gateway;

		private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = false
		};

		public PostCharacterUseCase(
			ILogger<PostCharacterUseCase> logger,
			ICharacterGateway gateway)
		{
			_logger = logger;
			_gateway = gateway;
		}

		public async Task<PostCharacterResponse> ExecuteAsync(PostCharacterRequest request)
		{
			// Log request
			_logger.LogInformation("Starting CreateCharacter request: {RequestJson}",
				JsonSerializer.Serialize(request, _jsonOptions));

			// Mapping DTO → Domain
			var character = request.ToDomain();

			// Save Character
			var createdCharacter = await _gateway.CreateAsync(character);

			// Mapping Domain → DTO
			var response = createdCharacter.ToPostResponse();

			// Log response
			_logger.LogInformation("Finished CreateCharacter response: {ResponseJson}",
			  JsonSerializer.Serialize(response, _jsonOptions));

			return response;
		}
	}

}