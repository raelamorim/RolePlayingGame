
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RolePlayingGame.Api.Controllers;
using RolePlayingGame.Application.Dtos.Request;
using RolePlayingGame.Application.Dtos.Response;
using RolePlayingGame.Application.Interfaces;

namespace RolePlayingGame.UnitTest.Api.Controllers
{
    public class BattleControllerTests
    {
		private BattleController CreateController(out IPostBattleUseCase useCase)
		{
			var logger = Substitute.For<ILogger<BattleController>>();
			useCase = Substitute.For<IPostBattleUseCase>();

			return new BattleController(logger);
		}

		[Fact(DisplayName = "StartBattle returns 200 OK with valid response")]
		public async Task StartBattle_ValidRequest_ReturnsOk()
		{
			// Arrange
			var controller = CreateController(out var useCase);

			var request = new PostBattleRequest
			{
				Character1Id = Guid.NewGuid(),
				Character2Id = Guid.NewGuid()
			};

			var expectedResponse = new PostBattleResponse
			{
				BattleLog = "Battle log here"
			};

			useCase.ExecuteAsync(request).Returns(expectedResponse);

			// Act
			var result = await controller.StartBattle(request, useCase);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var resp = Assert.IsType<PostBattleResponse>(okResult.Value);

			Assert.Equal(expectedResponse.BattleLog, resp.BattleLog);

			await useCase.Received(1).ExecuteAsync(request);
		}
	}
}
