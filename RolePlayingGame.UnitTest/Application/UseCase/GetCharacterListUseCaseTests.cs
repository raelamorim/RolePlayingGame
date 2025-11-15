using Microsoft.Extensions.Logging;
using NSubstitute;
using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Domain.Enums;
using RolePlayingGame.Application.UseCase;
using RolePlayingGame.Domain.Gateways;

namespace RolePlayingGame.UnitTest.Application.UseCase
{
    public class GetCharacterListUseCaseTests
    {
		private Character CreateCharacter(string name, Job job, int currentHp)
		{
			var c = new Character(name, job);

			// Reduce life until desired value
			int diff = c.CurrentHealthPoints - currentHp;

			if (diff > 0)
				c.ReduceHealth(diff);

			return c;
		}

		[Fact(DisplayName = "ExecuteAsync returns mapped list with correct fields")]
		public async Task ExecuteAsync_ReturnsMappedList_CorrectFields()
		{
			// Arrange
			var gateway = Substitute.For<ICharacterGateway>();
			var logger = Substitute.For<ILogger<GetCharacterListUseCase>>();
			var useCase = new GetCharacterListUseCase(logger, gateway);

			var c1 = CreateCharacter("John", Job.Warrior, 20);
			var c2 = CreateCharacter("Sarah", Job.Mage, 0); // dead

			var list = new List<Character> { c1, c2 };

			gateway.GetAllAsync().Returns(list);

			// Act
			var result = await useCase.ExecuteAsync();
			var resultList = result.ToList();

			// Assert
			Assert.Equal(2, resultList.Count);

			var r1 = resultList[0];
			Assert.Equal(c1.Id, r1.Id);
			Assert.Equal("John", r1.Name);
			Assert.Equal("Warrior", r1.Job);
			Assert.Equal("Alive", r1.Status);

			var r2 = resultList[1];
			Assert.Equal(c2.Id, r2.Id);
			Assert.Equal("Sarah", r2.Name);
			Assert.Equal("Mage", r2.Job);
			Assert.Equal("Dead", r2.Status);

			await gateway.Received(1).GetAllAsync();
		}

		[Fact(DisplayName = "ExecuteAsync returns empty list when no characters exist")]
		public async Task ExecuteAsync_ReturnsEmptyList()
		{
			// Arrange
			var gateway = Substitute.For<ICharacterGateway>();
			var logger = Substitute.For<ILogger<GetCharacterListUseCase>>();
			var useCase = new GetCharacterListUseCase(logger, gateway);

			gateway.GetAllAsync().Returns(new List<Character>());

			// Act
			var result = await useCase.ExecuteAsync();

			// Assert
			Assert.NotNull(result);
			Assert.Empty(result);

			await gateway.Received(1).GetAllAsync();
		}
	}
}
