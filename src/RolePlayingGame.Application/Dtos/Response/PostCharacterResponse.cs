namespace RolePlayingGame.Application.Dtos.Response
{
    public class PostCharacterResponse
    {
		public string Name { get; set; } = string.Empty;
		public string Job { get; set; }
		public int HealthPoints { get; set; }
		public int Strength { get; set; }
		public int Dexterity { get; set; }
		public int Intelligence { get; set; }
		public double AttackModifier { get; set; }
		public double SpeedModifier { get; set; }
	}
}
