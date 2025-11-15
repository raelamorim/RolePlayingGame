namespace RolePlayingGame.Application.Dtos.Response
{
    public class GetCharacterDetailResponse
    {
		public Guid Id { get; set; }
		public string Name { get; set; } = "";
		public string Job { get; set; } = "";

		// Life
		public int CurrentHealthPoints { get; set; }
		public int MaximumHealthPoints { get; set; }

		// Stats
		public int Strength { get; set; }
		public int Dexterity { get; set; }
		public int Intelligence { get; set; }

		// Modifiers
		public double AttackModifier { get; set; }
		public double SpeedModifier { get; set; }
	}
}
