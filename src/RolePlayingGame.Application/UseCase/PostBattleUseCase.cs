using Microsoft.Extensions.Logging;
using RolePlayingGame.Application.Dtos.Request;
using RolePlayingGame.Application.Dtos.Response;
using RolePlayingGame.Application.Exceptions;
using RolePlayingGame.Application.Interfaces;
using RolePlayingGame.Application.Mappers;
using RolePlayingGame.Domain.Gateways;
using RolePlayingGame.Domain.ValueObjects;
using System.Text.Json;

namespace RolePlayingGame.Application.UseCase
{
	public class PostBattleUseCase : IPostBattleUseCase
	{
		private readonly ILogger<PostBattleUseCase> _logger;
		private readonly ICharacterGateway _gateway;

		private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = false
		};

		public PostBattleUseCase(
			ILogger<PostBattleUseCase> logger,
			ICharacterGateway gateway)
		{
			_logger = logger;
			_gateway = gateway;
		}

		public async Task<PostBattleResponse> ExecuteAsync(PostBattleRequest request)
		{
			// Log request
			_logger.LogInformation("Starting PostBattle request: {RequestJson}",
				JsonSerializer.Serialize(request, _jsonOptions));

			// Get Characters
			var player1 = await _gateway.GetByIdAsync(request.Character1Id)
				  ?? throw new PlayerNotFoundApplicationException(request.Character1Id);

			var player2 = await _gateway.GetByIdAsync(request.Character2Id)
						  ?? throw new PlayerNotFoundApplicationException(request.Character2Id);

			// Execute battle
			var battle = new Battle(player1, player2);
			var result = battle.ExecuteBattle();

			// Update character
			await _gateway.UpdateAsync(player1);
			await _gateway.UpdateAsync(player2);

			// Mapping Domain → DTO
			var response = result.Log.ToPostReponse();

			// Log response
			_logger.LogInformation("Finished request response: {ResponseJson}",
			  JsonSerializer.Serialize(response, _jsonOptions));

			return response;

		}
	}
}
