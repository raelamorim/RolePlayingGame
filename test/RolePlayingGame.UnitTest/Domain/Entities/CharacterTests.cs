using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Domain.Enums;
using RolePlayingGame.Domain.Enums;

namespace RolePlayingGame.UnitTest.Domain.Entities
{
    public class CharacterTests
    {
		[Fact(DisplayName = "Constructor creates Warrior with correct base stats")]
		public void Constructor_Warrior_SetsExpectedStats()
		{
			// Act
			var character = new Character("Aragorn", Job.Warrior);

			// Assert
			Assert.NotEqual(Guid.Empty, character.Id);
			Assert.Equal("Aragorn", character.Name);
			Assert.Equal(Job.Warrior, character.Job);

			Assert.Equal(20, character.MaximumHealthPoints);
			Assert.Equal(10, character.Strength);
			Assert.Equal(5, character.Dexterity);
			Assert.Equal(5, character.Intelligence);

			Assert.Equal((0.8 * 10) + (0.2 * 5), character.AttackModifier, 3);
			Assert.Equal(0.6 * 5, character.SpeedModifier, 3);

			Assert.Equal(1, character.Level);
		}

		[Fact(DisplayName = "Constructor creates Thief with correct base stats")]
		public void Constructor_Thief_SetsExpectedStats()
		{
			var c = new Character("Shadow", Job.Thief);

			Assert.Equal(15, c.MaximumHealthPoints);
			Assert.Equal(6, c.Strength);
			Assert.Equal(10, c.Dexterity);
			Assert.Equal(4, c.Intelligence);

			Assert.Equal((0.5 * 6) + (0.5 * 10), c.AttackModifier, 3);
			Assert.Equal(0.9 * 10, c.SpeedModifier, 3);
		}

		[Fact(DisplayName = "Constructor creates Mage with correct base stats")]
		public void Constructor_Mage_SetsExpectedStats()
		{
			var c = new Character("Gandalf", Job.Mage);

			Assert.Equal(12, c.MaximumHealthPoints);
			Assert.Equal(3, c.Strength);
			Assert.Equal(4, c.Dexterity);
			Assert.Equal(12, c.Intelligence);

			Assert.Equal(12, c.AttackModifier, 3); // Intelligence
			Assert.Equal(0.4 * 4, c.SpeedModifier, 3);
		}

		[Fact(DisplayName = "LevelUp increases level by 1")]
		public void LevelUp_IncrementsLevel()
		{
			var c = new Character("Test", Job.Warrior);

			var initial = c.Level;

			c.LevelUp();

			Assert.Equal(initial + 1, c.Level);
		}

		[Fact(DisplayName = "ChangeJob assigns new stats")]
		public void ChangeJob_ChangesStatsCorrectly()
		{
			var c = new Character("Hero", Job.Warrior);

			c.ChangeJob(Job.Mage);

			Assert.Equal(Job.Mage, c.Job);

			Assert.Equal(12, c.MaximumHealthPoints);
			Assert.Equal(3, c.Strength);
			Assert.Equal(4, c.Dexterity);
			Assert.Equal(12, c.Intelligence);

			Assert.Equal(12, c.AttackModifier, 3);
			Assert.Equal(0.4 * 4, c.SpeedModifier, 3);
		}

		[Fact(DisplayName = "Invalid job throws exception")]
		public void Constructor_InvalidJob_ThrowsException()
		{
			// Arrange
			var invalidJob = (Job)999;

			// Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				var c = new Character("Bug", invalidJob);
			});
		}

		[Fact(DisplayName = "ChangeJob with invalid job throws exception")]
		public void ChangeJob_InvalidJob_ThrowsException()
		{
			var c = new Character("Test", Job.Warrior);

			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				c.ChangeJob((Job)999);
			});
		}

		[Fact(DisplayName = "ReduceHealth decreases HP correctly")]
		public void ReduceHealth_DecreasesCorrectly()
		{
			var c = new Character("Aragorn", Job.Warrior);

			c.ReduceHealth(7);

			Assert.Equal(c.MaximumHealthPoints - 7, c.CurrentHealthPoints);
		}

		[Fact(DisplayName = "ReduceHealth cannot reduce HP below zero")]
		public void ReduceHealth_DoesNotGoBelowZero()
		{
			var c = new Character("MageGuy", Job.Mage);

			c.ReduceHealth(999);

			Assert.Equal(0, c.CurrentHealthPoints);
		}

		[Fact(DisplayName = "ReduceHealth with negative value throws exception")]
		public void ReduceHealth_Negative_ThrowsException()
		{
			var c = new Character("Thief", Job.Thief);

			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				c.ReduceHealth(-10);
			});
		}

		[Fact(DisplayName = "Status becomes Dead when HP reaches zero")]
		public void Status_IsDead_WhenHpZero()
		{
			var c = new Character("Shadow", Job.Thief);

			c.ReduceHealth(c.MaximumHealthPoints);

			Assert.Equal(Status.Dead, c.Status);
		}

		[Fact(DisplayName = "Status remains Alive when HP above zero")]
		public void Status_IsAlive_WhenHpAboveZero()
		{
			var c = new Character("Shadow", Job.Thief);

			c.ReduceHealth(5);

			Assert.Equal(Status.Alive, c.Status);
		}

		[Fact(DisplayName = "ChangeJob keeps CurrentHealthPoints within new limits")]
		public void ChangeJob_AdjustsHealthWithinNewMax()
		{
			var c = new Character("Hero", Job.Warrior);
			c.ReduceHealth(15); // HP = 5

			// Mage max HP = 12
			c.ChangeJob(Job.Mage);

			Assert.True(c.CurrentHealthPoints <= c.MaximumHealthPoints);
			Assert.Equal(5, c.CurrentHealthPoints); // still valid for Mage
		}

		[Fact(DisplayName = "ChangeJob clamps HP to new max if necessary")]
		public void ChangeJob_ClampsHealth_WhenNewMaxLower()
		{
			var c = new Character("Tanky", Job.Warrior);
			// Warrior max = 20
			// Mage max = 12
			// Force invalid future case
			c.ReduceHealth(0); // HP still 20

			c.ChangeJob(Job.Mage);

			Assert.Equal(12, c.CurrentHealthPoints);
		}

		[Fact(DisplayName = "LevelUp does not modify stats or HP")]
		public void LevelUp_DoesNotChangeStats()
		{
			var c = new Character("Test", Job.Thief);

			int hp = c.CurrentHealthPoints;
			int str = c.Strength;
			int dex = c.Dexterity;
			int intl = c.Intelligence;

			c.LevelUp();

			Assert.Equal(str, c.Strength);
			Assert.Equal(dex, c.Dexterity);
			Assert.Equal(intl, c.Intelligence);
			Assert.Equal(hp, c.CurrentHealthPoints);
		}
	}
}
