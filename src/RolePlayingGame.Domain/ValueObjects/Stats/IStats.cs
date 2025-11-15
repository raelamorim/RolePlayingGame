namespace RolePlayingGame.Domain.ValueObjects.Stats
{
	public interface IStats
	{
		int MaximumHealthPoints { get; }
		int Strength { get; }
		int Dexterity { get; }
		int Intelligence { get; }

		double AttackModifier { get; }
		double SpeedModifier { get; }
	}
}
