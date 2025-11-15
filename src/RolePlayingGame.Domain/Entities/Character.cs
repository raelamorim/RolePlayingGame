using RolePlayingGame.Application.Domain.Enums;
using RolePlayingGame.Domain.Enums;

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


		// Core attributes (future evolutions)
		public int MaximumHealthPoints { get; private set; }
		public int Strength { get; private set; }
		public int Dexterity { get; private set; }
		public int Intelligence { get; private set; }

		// Modifiers (from the job)
		public double AttackModifier { get; private set; }
		public double SpeedModifier { get; private set; }

		// Leveling (future feature)
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
			switch (job)
			{
				case Job.Warrior:
					MaximumHealthPoints = 20;
					Strength = 10;
					Dexterity = 5;
					Intelligence = 5;
					AttackModifier = (0.8 * Strength) + (0.2 * Dexterity);
					SpeedModifier = 0.6 * Dexterity;
					break;

				case Job.Thief:
					MaximumHealthPoints = 15;
					Strength = 6;
					Dexterity = 10;
					Intelligence = 4;
					AttackModifier = (0.5 * Strength) + (0.5 * Dexterity);
					SpeedModifier = 0.9 * Dexterity;
					break;

				case Job.Mage:
					MaximumHealthPoints = 12;
					Strength = 3;
					Dexterity = 4;
					Intelligence = 12;
					AttackModifier = Intelligence;
					SpeedModifier = 0.4 * Dexterity;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(job), "Invalid job type.");
			}
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
