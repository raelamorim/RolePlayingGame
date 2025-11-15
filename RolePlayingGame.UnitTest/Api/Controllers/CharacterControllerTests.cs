
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RolePlayingGame.Api.Controllers;
using RolePlayingGame.Application.Dtos.Request;
using RolePlayingGame.Application.Dtos.Response;
using RolePlayingGame.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RolePlayingGame.UnitTest.Api.Controllers
{
	public class CharacterControllerTests
	{
		// ----------------------------
		// POST /characters
		// ----------------------------
		private CharacterController CreateController()
		{
			var logger = Substitute.For<ILogger<CharacterController>>();
			var controller = new CharacterController(logger)
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = new DefaultHttpContext()
				}
			};
			return controller;
		}

		private IList<ValidationResult> ValidateModel(object model)
		{
			var context = new ValidationContext(model);
			var results = new List<ValidationResult>();
			Validator.TryValidateObject(model, context, results, validateAllProperties: true);
			return results;
		}

		[Theory(DisplayName = "CreateCharacter returns 400 BadRequest when name or job invalid")]

		// Invalid name
		[InlineData(null, "Warrior")]              // null name
		[InlineData("", "Warrior")]                // empty
		[InlineData("   ", "Warrior")]             // whitespace
		[InlineData("A", "Warrior")]               // short
		[InlineData("ThisNameIsWayTooLongForTheGameLimits", "Warrior")]
		// Invalid Job
		[InlineData("ValidName", null)]
		[InlineData("ValidName", "")]
		[InlineData("ValidName", " ")]
		[InlineData("ValidName", "InvalidJob")]
		// Both Invalid
		[InlineData("", "")]
		public async Task CreateCharacter_InvalidRequest_ReturnsBadRequestAsync(string name, string job)
		{
			// Arrange
			var controller = CreateController();
			var request = new PostCharacterRequest { Name = name, Job = job };

			var validationResults = ValidateModel(request);
			foreach (var validationResult in validationResults)
			{
				controller.ModelState.AddModelError(
					validationResult.MemberNames != null ? string.Join(",", validationResult.MemberNames) : "",
					validationResult.ErrorMessage ?? "");
			}

			var service = Substitute.For<IPostCharacterUseCase>();

			// Act
			var result = await controller.CreateCharacterAsync(request, service);

			// Assert
			var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
			Assert.NotNull(badRequest.Value);
		}

		[Fact(DisplayName = "CreateCharacter returns 200 OK for valid request")]
		public async Task CreateCharacter_ValidRequest_ReturnsOkAsync()
		{
			// Arrange
			var controller = CreateController();
			var request = new PostCharacterRequest { Name = "Valid_Name", Job = "Warrior" };

			var validationResults = ValidateModel(request);
			Assert.Empty(validationResults);

			var service = Substitute.For<IPostCharacterUseCase>();
			service.ExecuteAsync(request)
				   .Returns(new PostCharacterResponse { Name = request.Name, Job = request.Job });

			// Act
			var result = await controller.CreateCharacterAsync(request, service);

			// Assert
			var ok = Assert.IsType<OkObjectResult>(result.Result);
			var resp = Assert.IsType<PostCharacterResponse>(ok.Value);

			Assert.Equal(request.Name, resp.Name);
			Assert.Equal(request.Job, resp.Job);

			// Confirms that the service was called once
			await service.Received(1).ExecuteAsync(request);
		}

		// ----------------------------
		// GET /characters
		// ----------------------------
		[Fact(DisplayName = "GetCharacters returns 200 OK with character list")]
		public async Task GetCharacters_ReturnsOk_WithList()
		{
			// Arrange
			var controller = CreateController();

			var useCase = Substitute.For<IGetCharacterListUseCase>();
			var expectedList = new List<GetCharacterListResponse>
			{
				new GetCharacterListResponse
				{
					Id = Guid.NewGuid(),
					Name = "John",
					Job = "Warrior",
					Status = "Alive"
				},
				new GetCharacterListResponse
				{
					Id = Guid.NewGuid(),
					Name = "Sara",
					Job = "Mage",
					Status = "Dead"
				}
			};

			useCase.ExecuteAsync().Returns(expectedList);

			// Act
			var result = await controller.GetCharactersAsync(useCase);

			// Assert
			var ok = Assert.IsType<OkObjectResult>(result.Result);
			var listResult = Assert.IsAssignableFrom<IEnumerable<GetCharacterListResponse>>(ok.Value);

			Assert.Equal(2, listResult.Count());

			await useCase.Received(1).ExecuteAsync();
		}

		[Fact(DisplayName = "GetCharacters returns 200 OK with empty list")]
		public async Task GetCharacters_ReturnsOk_EmptyList()
		{
			// Arrange
			var controller = CreateController();

			var useCase = Substitute.For<IGetCharacterListUseCase>();
			useCase.ExecuteAsync().Returns(new List<GetCharacterListResponse>());

			// Act
			var result = await controller.GetCharactersAsync(useCase);

			// Assert
			var ok = Assert.IsType<OkObjectResult>(result.Result);
			var list = Assert.IsAssignableFrom<IEnumerable<GetCharacterListResponse>>(ok.Value);
			Assert.Empty(list);

			await useCase.Received(1).ExecuteAsync();
		}

		// ----------------------------
		// GET /characters/{id}
		// ----------------------------
		[Fact(DisplayName = "GetCharacter returns 200 OK when character exists")]
		public async Task GetCharacter_ReturnsOk_WhenFound()
		{
			// Arrange
			var controller = CreateController();
			var id = Guid.NewGuid();

			var useCase = Substitute.For<IGetCharacterDetailUseCase>();

			var expected = new GetCharacterDetailResponse
			{
				Id = id,
				Name = "Alice",
				Job = "Thief",
				MaximumHealthPoints = 15,
				CurrentHealthPoints = 15,
				Strength = 6,
				Dexterity = 10,
				Intelligence = 4,
				AttackModifier = 7.5,
				SpeedModifier = 9.0
			};

			useCase.ExecuteAsync(id).Returns(expected);

			// Act
			var result = await controller.GetCharacterAsync(id, useCase);

			// Assert
			var ok = Assert.IsType<OkObjectResult>(result.Result);
			var detail = Assert.IsType<GetCharacterDetailResponse>(ok.Value);

			Assert.Equal(id, detail.Id);

			await useCase.Received(1).ExecuteAsync(id);
		}

		[Fact(DisplayName = "GetCharacter returns 404 NotFound when character does not exist")]
		public async Task GetCharacter_ReturnsNotFound_WhenNull()
		{
			// Arrange
			var controller = CreateController();
			var id = Guid.NewGuid();

			var useCase = Substitute.For<IGetCharacterDetailUseCase>();
			useCase.ExecuteAsync(id).Returns((GetCharacterDetailResponse?)null);

			// Act
			var result = await controller.GetCharacterAsync(id, useCase);

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);

			await useCase.Received(1).ExecuteAsync(id);
		}
	}
}
