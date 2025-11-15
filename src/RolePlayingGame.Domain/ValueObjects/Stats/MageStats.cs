namespace RolePlayingGame.Domain.ValueObjects.Stats
{
	public class MageStats : IStats
	{
		public int MaximumHealthPoints => 12;
		public int Strength => 3;
		public int Dexterity => 4;
		public int Intelligence => 12;

		public double AttackModifier => Intelligence;
		public double SpeedModifier => 0.4 * Dexterity;
	}

}
