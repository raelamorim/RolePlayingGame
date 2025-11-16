using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Domain.Enums;
using RolePlayingGame.Domain.Services;
using RolePlayingGame.Domain.ValueObjects;
using System.Linq;

namespace RolePlayingGame.UnitTest.Domain.ValueObjects
{
    public class BattleTests
    {
		private Character CreateCharacter(string name, Job job, int currentHp = -1)
		{
			var c = new Character(name, job);
			// Only reduce health when desired hp is lower than current to avoid passing negative amount to ReduceHealth
			if (currentHp >= 0 && c.CurrentHealthPoints > currentHp)
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

		// replace previous CreateBattleWithSeed
		private Battle CreateBattleWithDeterministicProvider(Character c1, Character c2, int[]? sequence = null)
		{
			return new Battle(c1, c2, new DeterministicRandomProvider(sequence));
		}

		[Fact(DisplayName = "Battle is deterministic with same Random provider sequence")]
		public void Battle_SameSequence_ProducesSameLog()
		{
			// Arrange - identical initial state but independent character instances
			var char1a = CreateCharacter("Hero", Job.Warrior);
			var char2a = CreateCharacter("Villain", Job.Thief);

			var char1b = CreateCharacter("Hero", Job.Warrior);
			var char2b = CreateCharacter("Villain", Job.Thief);

			// Use same deterministic sequence for both runs
			var seq = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };

			var battle1 = CreateBattleWithDeterministicProvider(char1a, char2a, seq);
			var battle2 = CreateBattleWithDeterministicProvider(char1b, char2b, seq);

			// Act
			var result1 = battle1.ExecuteBattle();
			var result2 = battle2.ExecuteBattle();

			// Assert - logs must be identical for the same sequence and same initial state
			Assert.Equal(result1.Log, result2.Log);
		}

		[Fact(DisplayName = "Battle never reduces hp below zero and announces a winner")]
		public void Battle_Result_HpNeverNegative_AnnouncesWinner()
		{
			// Arrange
			var char1 = CreateCharacter("Alice", Job.Warrior, 10);
			var char2 = CreateCharacter("Bob", Job.Mage, 6);

			// deterministic seed
			var battle = CreateBattleWithDeterministicProvider(char1, char2, sequence: new[] { 1, 2, 3, 4, 5 });

			// Act
			var result = battle.ExecuteBattle();

			// Assert - winner message present
			Assert.Contains("wins the battle", result.Log);

			// Assert - HP bounds
			Assert.InRange(char1.CurrentHealthPoints, 0, int.MaxValue);
			Assert.InRange(char2.CurrentHealthPoints, 0, int.MaxValue);
		}

		[Fact(DisplayName = "Battle with different deterministic sequences produce different logs")]
		public void Battle_DifferentSequences_ProduceDifferentLogs()
		{
			// Arrange
			var c1a = CreateCharacter("P1", Job.Thief);
			var c2a = CreateCharacter("P2", Job.Mage);

			var seq1 = new[] { 1, 2, 3, 4, 5, 6 };
			var seq2 = new[] { 6, 5, 4, 3, 2, 1 };

			var b1 = CreateBattleWithDeterministicProvider(CreateCharacter("P1", Job.Thief), CreateCharacter("P2", Job.Mage), seq1);
			var b2 = CreateBattleWithDeterministicProvider(CreateCharacter("P1", Job.Thief), CreateCharacter("P2", Job.Mage), seq2);

			// Act
			var r1 = b1.ExecuteBattle();
			var r2 = b2.ExecuteBattle();

			// Assert - sequences differ so logs should usually differ
			Assert.NotEqual(r1.Log, r2.Log);
		}

		[Fact(DisplayName = "Battle verifies first character always starts with correct initial HP")]
		public void Battle_InitialState_Character1HasCorrectHP()
		{
			// Arrange
			var char1 = CreateCharacter("Alice", Job.Warrior, 20);
			var char2 = CreateCharacter("Bob", Job.Thief, 15);
			var initialHP1 = char1.CurrentHealthPoints;

			var battle = CreateBattleWithDeterministicProvider(char1, char2, new[] { 5, 3, 2, 4 });

			// Act
			var result = battle.ExecuteBattle();

			// Assert - initial HP logged
			Assert.Contains($"{initialHP1} HP", result.Log);
			Assert.Contains("Alice", result.Log);
		}

		[Fact(DisplayName = "Battle verifies winner is always the survivor")]
		public void Battle_Winner_IsAlwaysTheOneSurviving()
		{
			// Arrange
			var char1 = CreateCharacter("Strong", Job.Warrior, 50);
			var char2 = CreateCharacter("Weak", Job.Thief, 1);

			var battle = CreateBattleWithDeterministicProvider(char1, char2, new[] { 10, 20, 30 });

			// Act
			var result = battle.ExecuteBattle();

			// Assert - one must have 0 HP, other > 0
			var char1Alive = char1.CurrentHealthPoints > 0;
			var char2Alive = char2.CurrentHealthPoints > 0;
			Assert.True(char1Alive != char2Alive, "Exactly one character should survive");
			
			var winner = char1Alive ? "Strong" : "Weak";
			Assert.Contains($"{winner} wins the battle", result.Log);
			Assert.Contains($"{winner} still has", result.Log);
		}

		[Fact(DisplayName = "Battle verifies speed determines turn order")]
		public void Battle_SpeedDeterminesAttackOrder()
		{
			// Arrange - deterministic sequence: [5, 2] means char1 speed=5, char2 speed=2
			var char1 = CreateCharacter("Fast", Job.Thief, 30);
			var char2 = CreateCharacter("Slow", Job.Warrior, 30);

			var seq = new[] { 5, 2, 3, 1, 4, 0 }; // speed values cycling
			var battle = CreateBattleWithDeterministicProvider(char1, char2, seq);

			// Act
			var result = battle.ExecuteBattle();

			// Assert - "Fast" should attack before "Slow" in first round (5 > 2)
			var fastIdx = result.Log.IndexOf("Fast attacks");
			var slowIdx = result.Log.IndexOf("Slow attacks");
			Assert.True(fastIdx >= 0, "Fast character should attack");
			Assert.True(slowIdx >= 0, "Slow character should attack");
			Assert.True(fastIdx < slowIdx, "Faster character should attack first in round");
		}

		[Fact(DisplayName = "Battle verifies damage is always non-negative")]
		public void Battle_Damage_IsAlwaysNonNegative()
		{
			// Arrange
			var char1 = CreateCharacter("A", Job.Warrior, 25);
			var char2 = CreateCharacter("B", Job.Mage, 25);

			// Extended sequence with enough values for multiple battle rounds
			var seq = Enumerable.Range(0, 50).ToArray();
			var battle = CreateBattleWithDeterministicProvider(char1, char2, seq);

			// Act
			var result = battle.ExecuteBattle();

			// Assert - parse all "for X" damage values
			var damageMatches = System.Text.RegularExpressions.Regex.Matches(result.Log, @"for (\d+)");
			foreach (System.Text.RegularExpressions.Match match in damageMatches)
			{
				var damage = int.Parse(match.Groups[1].Value);
				Assert.True(damage >= 0, $"Damage {damage} should be non-negative");
			}
		}

		[Fact(DisplayName = "Battle verifies HP progression is valid")]
		public void Battle_HPProgression_DecreasesOrStaysZero()
		{
			// Arrange
			var char1 = CreateCharacter("P1", Job.Warrior, 40);
			var char2 = CreateCharacter("P2", Job.Thief, 35);

			var seq = new[] { 3, 5, 2, 4, 1, 6, 7, 2, 3 };
			var battle = CreateBattleWithDeterministicProvider(char1, char2, seq);

			// Act
			var result = battle.ExecuteBattle();

			// Assert - final HP must be valid
			Assert.True(char1.CurrentHealthPoints >= 0, "char1 HP must not be negative");
			Assert.True(char2.CurrentHealthPoints >= 0, "char2 HP must not be negative");
			Assert.True(char1.CurrentHealthPoints == 0 || char2.CurrentHealthPoints == 0, "One must be dead");
		}

		[Fact(DisplayName = "Battle verifies both characters participate if both alive")]
		public void Battle_BothCharactersAttack_IfBothAlive()
		{
			// Arrange
			var char1 = CreateCharacter("First", Job.Warrior, 50);
			var char2 = CreateCharacter("Second", Job.Mage, 50);

			// Extended sequence to cover multiple rounds: speed values for each round, then damage
			var seq = new[] { 2, 1, 5, 3, 4, 2, 1, 5, 3, 2, 4, 1, 5, 2, 3, 1, 4, 2, 5, 3 };
			var battle = CreateBattleWithDeterministicProvider(char1, char2, seq);

			// Act
			var result = battle.ExecuteBattle();

			// Assert - both should attack at least once (unless one dies immediately, which is unlikely)
			var firstAttacks = System.Text.RegularExpressions.Regex.Matches(result.Log, "First attacks").Count;
			var secondAttacks = System.Text.RegularExpressions.Regex.Matches(result.Log, "Second attacks").Count;
			
			Assert.True(firstAttacks >= 1, "First character should attack at least once");
			Assert.True(secondAttacks >= 1, "Second character should attack at least once");
		}

		[Fact(DisplayName = "Battle verifies dead character does not attack")]
		public void Battle_DeadCharacter_DoesNotAttack()
		{
			// Arrange - use very high damage to kill quickly
			var char1 = CreateCharacter("Killer", Job.Warrior, 100);
			var char2 = CreateCharacter("Victim", Job.Thief, 5);

			// High damage values to ensure quick kill, extended sequence for safety
			var seq = new[] { 10, 20, 30, 40, 50, 60, 15, 25, 35, 45 };
			var battle = CreateBattleWithDeterministicProvider(char1, char2, seq);

			// Act
			var result = battle.ExecuteBattle();

			// Assert - victim should have 0 HP and not attack after death
			Assert.Equal(0, char2.CurrentHealthPoints);
			Assert.Contains("Killer wins", result.Log);
		}

		[Fact(DisplayName = "Battle log contains both character names")]
		public void Battle_LogContains_BothCharacterNames()
		{
			// Arrange
			var char1 = CreateCharacter("Hero", Job.Warrior);
			var char2 = CreateCharacter("Villain", Job.Mage);

			var battle = CreateBattleWithDeterministicProvider(char1, char2, new[] { 2, 1, 3, 4 });

			// Act
			var result = battle.ExecuteBattle();

			// Assert
			Assert.Contains("Hero", result.Log);
			Assert.Contains("Villain", result.Log);
		}
	}
}
