using FluentAssertions;
using Reqnroll;
using RolePlayingGame.IntegrationTest.Support;
using System.Text.Json;

[Binding]
public class BattleSteps
{
	private readonly TestContext _context;

	public BattleSteps(TestContext context)
	{
		_context = context;
	}

	// ------------------------------------------------------------
	// GIVEN: Battle Request using two created characters
	// ------------------------------------------------------------
	[Given(@"I have a battle request using these two characters")]
	public void GivenBattleRequest()
	{
		if (_context.CreatedCharacters.Count < 2)
			throw new Exception("Two characters must exist for battle.");

		var body = new
		{
			character1Id = _context.CreatedCharacters[0].Id,
			character2Id = _context.CreatedCharacters[1].Id
		};

		_context.LastRequestBody = JsonSerializer.Serialize(body);
	}

	// ------------------------------------------------------------
	// GIVEN: Battle Request with explicit ids
	// ------------------------------------------------------------
	[Given(@"I have a battle request using character (.*) and nonexistent character (.*)")]
	public void GivenBattleRequestWithMissing(string id1, string id2)
	{
		var body = new
		{
			character1Id = id1,
			character2Id = id2
		};

		_context.LastRequestBody = JsonSerializer.Serialize(body);
	}

	[Given(@"I have a battle request using character (.*) and the same character (.*)")]
	public void GivenBattleRequestWithSameId(int id1, int id2)
	{
		var body = new
		{
			character1Id = id1,
			character2Id = id2
		};

		_context.LastRequestBody = JsonSerializer.Serialize(body);
	}

	// ------------------------------------------------------------
	// THEN: Battle Log
	// ------------------------------------------------------------
	[Then(@"the battle log should not be empty")]
	public async Task ThenBattleLogShouldNotBeEmpty()
	{
		var body = await _context.LastResponse!.Content.ReadAsStringAsync();

		var json = JsonSerializer.Deserialize<Dictionary<string, object>>(body)!;

		json.Should().ContainKey("battleLog");

		var log = json["battleLog"].ToString();

		log.Should().NotBeNullOrWhiteSpace();
	}
}
