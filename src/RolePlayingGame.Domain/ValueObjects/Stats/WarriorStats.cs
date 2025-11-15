namespace RolePlayingGame.Domain.ValueObjects.Stats
{
	public class WarriorStats : IStats
	{
		public int MaximumHealthPoints => 20;
		public int Strength => 10;
		public int Dexterity => 5;
		public int Intelligence => 5;

		public double AttackModifier => (0.8 * Strength) + (0.2 * Dexterity);
		public double SpeedModifier => 0.6 * Dexterity;
	}

}
