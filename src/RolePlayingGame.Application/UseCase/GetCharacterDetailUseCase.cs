using Microsoft.Extensions.Logging;
using RolePlayingGame.Application.Dtos.Response;
using RolePlayingGame.Application.Exceptions;
using RolePlayingGame.Application.Interfaces;
using RolePlayingGame.Application.Mappers;
using RolePlayingGame.Domain.Gateways;
using System.Text.Json;

namespace RolePlayingGame.Application.UseCase
{
    public class GetCharacterDetailUseCase : IGetCharacterDetailUseCase
	{
		private readonly ILogger<GetCharacterDetailUseCase> _logger;
		private readonly ICharacterGateway _gateway;

		private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = false
		};

		public GetCharacterDetailUseCase(
			ILogger<GetCharacterDetailUseCase> logger, 
			ICharacterGateway gateway)
		{
			_logger = logger;
			_gateway = gateway;
		}

		public async Task<GetCharacterDetailResponse?> ExecuteAsync(Guid id)
		{
			// Log request
			_logger.LogInformation("Starting GetCharacterDetail id: {id}", id);

			// Get Character
			var character = await _gateway.GetByIdAsync(id) ?? throw new PlayerNotFoundApplicationException(id); ;

			// Mapping Domain → DTO
			var response = character.ToDetailResponse();

			// Log response
			_logger.LogInformation("Finished GetCharacterDetail response: {json}",
			  JsonSerializer.Serialize(response, _jsonOptions));

			return response;
		}
	}
}
