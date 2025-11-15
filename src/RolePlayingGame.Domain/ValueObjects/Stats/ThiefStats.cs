namespace RolePlayingGame.Domain.ValueObjects.Stats
{
	public class ThiefStats : IStats
	{
		public int MaximumHealthPoints => 15;
		public int Strength => 6;
		public int Dexterity => 10;
		public int Intelligence => 4;

		public double AttackModifier => (0.5 * Strength) + (0.5 * Dexterity);
		public double SpeedModifier => 0.9 * Dexterity;
	}

}
