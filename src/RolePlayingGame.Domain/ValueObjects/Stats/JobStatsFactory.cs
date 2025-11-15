using RolePlayingGame.Application.Domain.Enums;

namespace RolePlayingGame.Domain.ValueObjects.Stats
{
	public static class JobStatsFactory
	{
		public static IStats Create(Job job) =>
			job switch
			{
				Job.Warrior => new WarriorStats(),
				Job.Thief => new ThiefStats(),
				Job.Mage => new MageStats(),
				_ => throw new ArgumentOutOfRangeException(nameof(job))
			};
	}

}
