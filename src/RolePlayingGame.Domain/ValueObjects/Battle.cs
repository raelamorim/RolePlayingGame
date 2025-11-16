using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Domain.Services;
using System.Text;

namespace RolePlayingGame.Domain.ValueObjects
{
    public class Battle
    {
		private readonly Character _char1;
		private readonly Character _char2;
		private readonly IRandomProvider _random;
		private readonly StringBuilder _log = new();
		
		public Battle(Character char1, Character char2, IRandomProvider? randomProvider = null)
		{
			_char1 = char1;
			_char2 = char2;
			_random = randomProvider ?? new DefaultRandomProvider();
		}

		public BattleResult ExecuteBattle()
		{
			_log.AppendLine($"Battle between {_char1.Name} ({_char1.Job}) - {_char1.CurrentHealthPoints} HP and {_char2.Name} ({_char2.Job}) - {_char2.CurrentHealthPoints} HP begins!");

			while (_char1.CurrentHealthPoints > 0 && _char2.CurrentHealthPoints > 0)
			{
				ExecuteRound();
			}

			var winner = _char1.CurrentHealthPoints > 0 ? _char1 : _char2;

			_log.AppendLine($"{winner.Name} wins the battle! {winner.Name} still has {winner.CurrentHealthPoints} HP remaining!");

			return new BattleResult
			{
				Log = _log.ToString(),
			};
		}

		private void ExecuteRound()
		{
			Character first, second;
			int firstSpeed, secondSpeed;

			// Determine who goes first
			do
			{
				firstSpeed = _random.Next(0, (int)(_char1.SpeedModifier + 1));
				secondSpeed = _random.Next(0, (int)(_char2.SpeedModifier + 1));
			} while (firstSpeed == secondSpeed);

			if (firstSpeed > secondSpeed)
			{
				first = _char1;
				second = _char2;
			}
			else
			{
				first = _char2;
				second = _char1;
			}

			_log.AppendLine($"{first.Name} {firstSpeed} speed was faster than {second.Name} {secondSpeed} speed and will begin this round.");

			ExecuteTurn(first, second);
			if (second.CurrentHealthPoints > 0)
			{
				ExecuteTurn(second, first);
			}
		}

		private void ExecuteTurn(Character attacker, Character defender)
		{
			int damage = _random.Next(0, (int)(attacker.AttackModifier + 1));
			defender.ReduceHealth(damage);

			_log.AppendLine($"{attacker.Name} attacks {defender.Name} for {damage}, {defender.Name} has {defender.CurrentHealthPoints} HP remaining.");
		}
	}
}
