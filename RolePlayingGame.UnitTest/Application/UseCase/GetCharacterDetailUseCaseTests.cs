using Microsoft.Extensions.Logging;
using NSubstitute;
using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Domain.Enums;
using RolePlayingGame.Application.Dtos.Response;
using RolePlayingGame.Application.UseCase;
using RolePlayingGame.Domain.Gateways;

namespace RolePlayingGame.UnitTest.Application.UseCase
{
	public class GetCharacterDetailUseCaseTests
	{
		private Character CreateCharacter(string name, Job job, int currentHp)
		{
			var c = new Character(name, job);

			int diff = c.CurrentHealthPoints - currentHp;
			if (diff > 0)
				c.ReduceHealth(diff);

			return c;
		}

		[Fact(DisplayName = "ExecuteAsync returns mapped character when found")]
		public async Task ExecuteAsync_ReturnsMappedCharacter_WhenFound()
		{
			// Arrange
			var gateway = Substitute.For<ICharacterGateway>();
			var logger = Substitute.For<ILogger<GetCharacterDetailUseCase>>();
			var useCase = new GetCharacterDetailUseCase(logger, gateway);

			var id = Guid.NewGuid();

			var character = CreateCharacter("Alice", Job.Thief, 12);

			gateway.GetByIdAsync(id).Returns(character);

			// Act
			var result = await useCase.ExecuteAsync(id);

			// Assert
			Assert.NotNull(result);
			Assert.IsType<GetCharacterDetailResponse>(result);

			Assert.Equal(character.Id, result.Id);
			Assert.Equal(character.Name, result.Name);
			Assert.Equal(character.Job.ToString(), result.Job);

			// Life
			Assert.Equal(character.CurrentHealthPoints, result.CurrentHealthPoints);
			Assert.Equal(character.MaximumHealthPoints, result.MaximumHealthPoints);

			// Stats
			Assert.Equal(character.Strength, result.Strength);
			Assert.Equal(character.Dexterity, result.Dexterity);
			Assert.Equal(character.Intelligence, result.Intelligence);

			// Modifiers
			Assert.Equal(character.AttackModifier, result.AttackModifier);
			Assert.Equal(character.SpeedModifier, result.SpeedModifier);

			await gateway.Received(1).GetByIdAsync(id);
		}

		[Fact(DisplayName = "ExecuteAsync returns null when character not found")]
		public async Task ExecuteAsync_ReturnsNull_WhenNotFound()
		{
			// Arrange
			var gateway = Substitute.For<ICharacterGateway>();
			var logger = Substitute.For<ILogger<GetCharacterDetailUseCase>>();
			var useCase = new GetCharacterDetailUseCase(logger, gateway);

			var id = Guid.NewGuid();

			gateway.GetByIdAsync(id).Returns((Character?)null);

			// Act
			var result = await useCase.ExecuteAsync(id);

			// Assert
			Assert.Null(result);

			await gateway.Received(1).GetByIdAsync(id);
		}
	}
}
