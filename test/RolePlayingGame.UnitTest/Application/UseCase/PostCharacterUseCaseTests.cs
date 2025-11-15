using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Domain.Enums;
using RolePlayingGame.Application.Dtos.Request;
using RolePlayingGame.Application.Dtos.Response;
using RolePlayingGame.Application.UseCase;
using RolePlayingGame.Domain.Gateways;

namespace RolePlayingGame.UnitTest.Application.UseCase
{
    public class PostCharacterUseCaseTests
    {
		private PostCharacterUseCase CreateUseCase(
		 ILogger<PostCharacterUseCase>? logger = null,
		 ICharacterGateway? gateway = null)
		{
			logger ??= Substitute.For<ILogger<PostCharacterUseCase>>();
			gateway ??= Substitute.For<ICharacterGateway>();

			return new PostCharacterUseCase(logger, gateway);
		}

		[Fact(DisplayName = "ExecuteAsync should create character and return mapped response")]
		public async Task ExecuteAsync_ValidRequest_ReturnsResponse()
		{
			// Arrange
			var request = new PostCharacterRequest
			{
				Name = "Valid_User",
				Job = "Warrior"
			};

			var expectedDomain = new Character("Valid_User", Job.Warrior);
			var expectedResponse = new PostCharacterResponse
			{
				Name = expectedDomain.Name,
				Job = expectedDomain.Job.ToString(),
				HealthPoints = expectedDomain.MaximumHealthPoints,
				Strength = expectedDomain.Strength,
				Dexterity = expectedDomain.Dexterity,
				Intelligence = expectedDomain.Intelligence,
				AttackModifier = expectedDomain.AttackModifier,
				SpeedModifier = expectedDomain.SpeedModifier
			};

			var logger = Substitute.For<ILogger<PostCharacterUseCase>>();
			var gateway = Substitute.For<ICharacterGateway>();

			gateway.CreateAsync(Arg.Any<Character>())
				   .Returns(expectedDomain);

			var useCase = CreateUseCase(logger, gateway);

			// Act
			var result = await useCase.ExecuteAsync(request);

			// Assert – Response content
			Assert.Equal(expectedResponse.Name, result.Name);
			Assert.Equal(expectedResponse.Job, result.Job);
			Assert.Equal(expectedResponse.HealthPoints, result.HealthPoints);
			Assert.Equal(expectedResponse.Strength, result.Strength);
			Assert.Equal(expectedResponse.Dexterity, result.Dexterity);
			Assert.Equal(expectedResponse.Intelligence, result.Intelligence);
			Assert.Equal(expectedResponse.AttackModifier, result.AttackModifier);
			Assert.Equal(expectedResponse.SpeedModifier, result.SpeedModifier);

			// Assert – Gateway was called once with mapped domain character
			await gateway.Received(1).CreateAsync(Arg.Is<Character>(c =>
				c.Name == request.Name &&
				c.Job == Job.Warrior
			));

			// Assert – Logger logs request and response (2 logs)
			logger.ReceivedWithAnyArgs(2).Log(
				Arg.Any<LogLevel>(),
				Arg.Any<EventId>(),
				Arg.Any<object>(),
				Arg.Any<Exception?>(),
				Arg.Any<Func<object, Exception?, string>>()
			);
		}

		[Fact(DisplayName = "ExecuteAsync should throw when gateway throws")]
		public async Task ExecuteAsync_GatewayThrows_PropagatesException()
		{
			// Arrange
			var request = new PostCharacterRequest
			{
				Name = "Valid_User",
				Job = "Mage"
			};

			var logger = Substitute.For<ILogger<PostCharacterUseCase>>();
			var gateway = Substitute.For<ICharacterGateway>();

			gateway.CreateAsync(Arg.Any<Character>())
				   .ThrowsAsync(new InvalidOperationException("DB error"));

			var useCase = CreateUseCase(logger, gateway);

			// Act + Assert
			await Assert.ThrowsAsync<InvalidOperationException>(() =>
				useCase.ExecuteAsync(request));

			// Should still log request
			logger.ReceivedWithAnyArgs().Log(
				Arg.Any<LogLevel>(),
				Arg.Any<EventId>(),
				Arg.Any<object>(),
				Arg.Any<Exception?>(),
				Arg.Any<Func<object, Exception?, string>>()
			);
		}

		[Fact(DisplayName = "ExecuteAsync should throw when Job is invalid")]
		public async Task ExecuteAsync_InvalidJob_ThrowsArgumentException()
		{
			// Arrange
			var request = new PostCharacterRequest
			{
				Name = "User123",
				Job = "InvalidJobName" // <-- job inválido
			};

			var logger = Substitute.For<ILogger<PostCharacterUseCase>>();
			var gateway = Substitute.For<ICharacterGateway>();

			var useCase = CreateUseCase(logger, gateway);

			// Act
			var act = () => useCase.ExecuteAsync(request);

			// Assert
			var ex = await Assert.ThrowsAsync<ArgumentException>(act);
			Assert.Contains("Job", ex.Message, StringComparison.OrdinalIgnoreCase);

			// gateway NUNCA pode ser chamado
			await gateway.DidNotReceive().CreateAsync(Arg.Any<Character>());

			// logger registra ao menos a entrada
			logger.ReceivedWithAnyArgs().Log(
				Arg.Any<LogLevel>(),
				Arg.Any<EventId>(),
				Arg.Any<object>(),
				Arg.Any<Exception?>(),
				Arg.Any<Func<object, Exception?, string>>()
			);
		}

	}
}
