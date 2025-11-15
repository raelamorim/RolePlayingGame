using Microsoft.Extensions.Logging;
using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Dtos.Response;
using RolePlayingGame.Application.Interfaces;
using RolePlayingGame.Application.Mappers;
using RolePlayingGame.Domain.Gateways;
using System.Text.Json;

namespace RolePlayingGame.Application.UseCase
{
    public class GetCharacterListUseCase : IGetCharacterListUseCase
	{
		private readonly ILogger<GetCharacterListUseCase> _logger;
		private readonly ICharacterGateway _gateway;

		private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = false
		};

		public GetCharacterListUseCase(
			ILogger<GetCharacterListUseCase> logger, 
			ICharacterGateway gateway)
		{
			_logger = logger;
			_gateway = gateway;
		}

		public async Task<IEnumerable<GetCharacterListResponse>> ExecuteAsync()
		{
			// Log request
			_logger.LogInformation("Starting GetCharacterList");

			var characters = await _gateway.GetAllAsync();

			// Mapping Domain → DTO
			var response = characters.Select(c => c.ToListResponse());

			// Log response
			_logger.LogInformation("Finished GetCharacterList response: {json}",
				JsonSerializer.Serialize(response, _jsonOptions));

			return response;
		}
	}
}
