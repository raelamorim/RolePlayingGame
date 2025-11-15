using Microsoft.Extensions.Logging;
using NSubstitute;
using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Domain.Enums;
using RolePlayingGame.Application.Dtos.Request;
using RolePlayingGame.Application.Exceptions;
using RolePlayingGame.Application.UseCase;
using RolePlayingGame.Domain.Gateways;

namespace RolePlayingGame.UnitTest.Application.UseCase
{
	public class PostBattleUseCaseTests
	{
		private (PostBattleUseCase useCase,
				 ICharacterGateway gateway,
				 ILogger<PostBattleUseCase> logger)
			CreateUseCase()
		{
			var logger = Substitute.For<ILogger<PostBattleUseCase>>();
			var gateway = Substitute.For<ICharacterGateway>();

			var useCase = new PostBattleUseCase(logger, gateway);

			return (useCase, gateway, logger);
		}

		// -------------------------------------------------------------
		// SUCCESS TEST (Theory with different HP values)
		// -------------------------------------------------------------
		[Theory(DisplayName = "ExecuteAsync → Success returns mapped response and updates players")]
		[InlineData(10, 0)]
		[InlineData(5, 1)]
		[InlineData(1, 30)]
		public async Task ExecuteAsync_Success_ReturnsMappedResponse_AndUpdatesPlayers(
			int newHp1, int newHp2)
		{
			// Arrange
			var (useCase, gateway, _) = CreateUseCase();

			var request = new PostBattleRequest
			{
				Character1Id = Guid.NewGuid(),
				Character2Id = Guid.NewGuid()
			};

			var char1 = new Character("Hero", Job.Warrior);
			var char2 = new Character("Villain", Job.Mage);

			gateway.GetByIdAsync(request.Character1Id).Returns(char1);
			gateway.GetByIdAsync(request.Character2Id).Returns(char2);

			// Act
			var response = await useCase.ExecuteAsync(request);

			// Assert
			Assert.NotEmpty(response.BattleLog);

			await gateway.Received(2).UpdateAsync(Arg.Any<Character>());
		}

		// -------------------------------------------------------------
		// PLAYER NOT FOUND
		// -------------------------------------------------------------
		[Fact(DisplayName = "ExecuteAsync → Player1 not found throws PlayerNotFoundApplicationException")]
		public async Task ExecuteAsync_Player1NotFound_ThrowsException()
		{
			var (useCase, gateway, _) = CreateUseCase();

			var request = new PostBattleRequest
			{
				Character1Id = Guid.NewGuid(),
				Character2Id = Guid.NewGuid()
			};

			gateway.GetByIdAsync(request.Character1Id).Returns((Character)null);

			await Assert.ThrowsAsync<PlayerNotFoundApplicationException>(() =>
				useCase.ExecuteAsync(request));
		}

		[Fact(DisplayName = "ExecuteAsync → Player2 not found throws PlayerNotFoundApplicationException")]
		public async Task ExecuteAsync_Player2NotFound_ThrowsException()
		{
			var (useCase, gateway, _) = CreateUseCase();

			var request = new PostBattleRequest
			{
				Character1Id = Guid.NewGuid(),
				Character2Id = Guid.NewGuid()
			};

			gateway.GetByIdAsync(request.Character1Id).Returns(new Character("NotFound", Job.Mage));
			gateway.GetByIdAsync(request.Character2Id).Returns((Character)null);

			await Assert.ThrowsAsync<PlayerNotFoundApplicationException>(() =>
				useCase.ExecuteAsync(request));
		}
	}
}
