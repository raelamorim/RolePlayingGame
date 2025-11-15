using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Domain.Enums;
using RolePlayingGame.Domain.ValueObjects;

namespace RolePlayingGame.UnitTest.Domain.ValueObjects
{
    public class BattleTests
    {
		private Character CreateCharacter(string name, Job job, int currentHp = -1)
		{
			var c = new Character(name, job);
			if (currentHp >= 0)
				c.ReduceHealth(c.CurrentHealthPoints - currentHp);
			return c;
		}

		[Theory(DisplayName = "Battle resolves correctly for various job combinations")]
		[InlineData(Job.Warrior, Job.Thief)]
		[InlineData(Job.Mage, Job.Warrior)]
		[InlineData(Job.Thief, Job.Mage)]
		[InlineData(Job.Warrior, Job.Mage)]
		public void Battle_MultipleJobCombinations_WinnerIsCorrect(Job job1, Job job2)
		{
			// Arrange
			var char1 = CreateCharacter("Hero", job1);
			var char2 = CreateCharacter("Villain", job2);

			var battle = new Battle(char1, char2);

			// Act
			var result = battle.ExecuteBattle();

			// Assert
			Assert.Contains("wins the battle", result.Log);
			Assert.True(char1.CurrentHealthPoints == 0 || char2.CurrentHealthPoints == 0);
			Assert.True(char1.CurrentHealthPoints >= 0);
			Assert.True(char2.CurrentHealthPoints >= 0);
		}

		[Theory(DisplayName = "Battle respects initial HP and does not go below zero")]
		[InlineData(Job.Warrior, Job.Thief, 5, 3)]
		[InlineData(Job.Mage, Job.Mage, 12, 1)]
		[InlineData(Job.Thief, Job.Warrior, 8, 8)]
		public void Battle_WithCustomHp_HpDoesNotGoBelowZero(Job job1, Job job2, int hp1, int hp2)
		{
			// Arrange
			var char1 = CreateCharacter("Alice", job1, hp1);
			var char2 = CreateCharacter("Bob", job2, hp2);

			var battle = new Battle(char1, char2);

			// Act
			var result = battle.ExecuteBattle();

			// Assert
			Assert.True(char1.CurrentHealthPoints >= 0);
			Assert.True(char2.CurrentHealthPoints >= 0);
			Assert.Contains("wins the battle", result.Log);
		}
	}
}
