using RolePlayingGame.Application.Domain.Enums;
using RolePlayingGame.Domain.Enums;
using RolePlayingGame.Domain.ValueObjects.Stats;

namespace RolePlayingGame.Application.Domain.Entities
{
    public class Character
    {
		public Guid Id { get; private set; }
		public string Name { get; private set; } = string.Empty;
		public Job Job { get; private set; }

		// Status
		public Status Status => CurrentHealthPoints > 0 ? Status.Alive : Status.Dead;
		public int CurrentHealthPoints { get; private set; }

		// Core Attributtes
		public int MaximumHealthPoints { get; private set; }
		public int Strength { get; private set; }
		public int Dexterity { get; private set; }
		public int Intelligence { get; private set; }

		public double AttackModifier { get; private set; }
		public double SpeedModifier { get; private set; }

		public int Level { get; private set; } = 1;

		private Character() { } 

		public Character(string name, Job job)
		{
			Id = Guid.NewGuid();
			Name = name;
			Job = job;

			ApplyJobBaseStats(job);

			CurrentHealthPoints = MaximumHealthPoints;
		}

		private void ApplyJobBaseStats(Job job)
		{
			var stats = JobStatsFactory.Create(job);

			// Stats
			MaximumHealthPoints = stats.MaximumHealthPoints;
			Strength = stats.Strength;
			Dexterity = stats.Dexterity;
			Intelligence = stats.Intelligence;
			AttackModifier = stats.AttackModifier;
			SpeedModifier = stats.SpeedModifier;
		}


		public void LevelUp()
		{
			Level++;
		}

		public void ChangeJob(Job newJob)
		{
			int currentHpBefore = CurrentHealthPoints;
			Job = newJob;
			ApplyJobBaseStats(newJob);
			CurrentHealthPoints = Math.Min(currentHpBefore, MaximumHealthPoints);
		}

		public void ReduceHealth(int amount)
		{
			if (amount < 0)
				throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");

			CurrentHealthPoints = Math.Max(0, CurrentHealthPoints - amount);
		}
	}
}
