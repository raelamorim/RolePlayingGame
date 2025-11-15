using Reqnroll;
using RolePlayingGame.IntegrationTest.Support;

namespace RolePlayingGame.IntegrationTest.Steps
{
	[Binding]
	public class Hooks
	{
		private readonly TestContext _context;

		public Hooks(TestContext context)
		{
			_context = context;
		}

		[BeforeScenario]
		public void ResetState()
		{
			_context.LastResponse = null;
			_context.CreatedCharacters.Clear();
		}
	}
}
