using System.Text;
using System.Text.Json;
using FluentAssertions;
using Reqnroll;
using RolePlayingGame.IntegrationTest.Support;

namespace RolePlayingGame.AcceptanceTests.Steps;

[Binding]
public class CharacterSteps
{
	private readonly TestContext _context;

	public CharacterSteps(TestContext context)
	{
		_context = context;
	}

	// ------------------------------------------------------------
	// GIVEN: Prepare Character Body
	// ------------------------------------------------------------
	[Given(@"I have a character body:")]
	public void GivenIHaveACharacterBody(Table table)
	{
		var row = table.Rows[0];

		var body = new
		{
			name = row["name"],
			job = row["job"]
		};

		_context.LastRequestBody = JsonSerializer.Serialize(body);
	}

	// ------------------------------------------------------------
	// GIVEN: Create Character (POST)
	// Reutiliza o mesmo método de POST do When 
	// ------------------------------------------------------------
	[Given(@"I have created a character:")]
	public async Task GivenIHaveCreatedACharacter(Table table)
	{
		GivenIHaveACharacterBody(table);
		await WhenIPOSTTo("api/characters");

		// Store created character data
		var responseBody = await _context.LastResponse!.Content.ReadAsStringAsync();
		var json = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody)!;

		Guid id = new(json["id"].ToString());

		_context.CreatedCharacters.Add(new TestCreatedCharacter
		{
			Id = id,
			Name = table.Rows[0]["name"],
			Job = table.Rows[0]["job"]
		});
	}

	// ------------------------------------------------------------
	// WHEN: POST
	// ------------------------------------------------------------
	[When(@"I POST to ""(.*)""")]
	public async Task WhenIPOSTTo(string endpoint)
	{
		var content = new StringContent(
			_context.LastRequestBody ?? "",
			Encoding.UTF8,
			"application/json"
		);

		_context.LastResponse = await _context.Client.PostAsync(endpoint, content);
	}

	// ------------------------------------------------------------
	// WHEN: GET
	// ------------------------------------------------------------
	[When(@"I GET ""(.*)""")]
	public async Task WhenIGet(string endpoint)
	{
		// If endpoint has {id}, replace with recorded id
		if (endpoint.Contains("{id}"))
		{
			var created = _context.CreatedCharacters.LastOrDefault();
			created.Should().NotBeNull("a character must have been created before using {id}");

			endpoint = endpoint.Replace("{id}", created!.Id.ToString());
		}

		_context.LastResponse = await _context.Client.GetAsync(endpoint);
	}

	// ------------------------------------------------------------
	// THEN: Validate Status
	// ------------------------------------------------------------
	[Then(@"the response status should be (.*)")]
	public void ThenStatusShouldBe(int code)
	{
		((int)_context.LastResponse!.StatusCode)
			.Should().Be(code);
	}

	// ------------------------------------------------------------
	// THEN: Response Contains ID
	// ------------------------------------------------------------
	[Then(@"the response should contain a character id")]
	public async Task ThenResponseShouldContainCharacterId()
	{
		var body = await _context.LastResponse!.Content.ReadAsStringAsync();

		var json = JsonSerializer.Deserialize<Dictionary<string, object>>(body)!;

		json.Should().ContainKey("id");
		var id = Guid.Parse(new(json["id"]!.ToString()));
		id.Should().NotBe(Guid.Empty);
	}

	// ------------------------------------------------------------
	// THEN: Response Contains List
	// ------------------------------------------------------------
	[Then(@"the response should contain a list of characters")]
	public async Task ThenResponseShouldContainList()
	{
		var body = await _context.LastResponse!.Content.ReadAsStringAsync();

		var list = JsonSerializer.Deserialize<List<object>>(body);

		list.Should().NotBeNull();
	}

	[Then(@"the response should contain an empty character list")]
	public async Task ThenResponseShouldContainEmptyList()
	{
		var body = await _context.LastResponse!.Content.ReadAsStringAsync();

		var list = JsonSerializer.Deserialize<List<object>>(body);

		list.Should().NotBeNull();
		list!.Count.Should().Be(0);
	}

	// ------------------------------------------------------------
	// THEN: Name Validation
	// ------------------------------------------------------------
	[Then(@"the character name should be ""(.*)""")]
	public async Task ThenCharacterNameShouldBe(string expected)
	{
		var body = await _context.LastResponse!.Content.ReadAsStringAsync();

		var json = JsonSerializer.Deserialize<Dictionary<string, object>>(body)!;

		json["name"].ToString().Should().Be(expected);
	}
}
