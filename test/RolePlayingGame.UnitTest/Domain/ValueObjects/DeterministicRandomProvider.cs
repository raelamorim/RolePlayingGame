using System;
using System.Linq;
using RolePlayingGame.Domain.Services;

namespace RolePlayingGame.UnitTest.Domain.ValueObjects
{
	/// <summary>
	/// Deterministic provider used only by tests. It cycles a predefined sequence of integers
	/// and maps them into the requested range. No seed is required.
	/// </summary>
	public class DeterministicRandomProvider : IRandomProvider
	{
		private readonly int[] _sequence;
		private int _index;

		public DeterministicRandomProvider(int[]? sequence = null)
		{
			// default deterministic sequence 0,1,2,3,...
			if (sequence != null && sequence.Length > 0)
			{
				_sequence = sequence;
			}
			else
			{
				_sequence = Enumerable.Range(0, 1024).ToArray();
			}
			_index = 0;
		}

		public int Next(int minValue, int maxValue)
		{
			if (maxValue <= minValue) 
				return minValue;
			
			var val = _sequence[_index++ % _sequence.Length];
			var range = maxValue - minValue;
			var mapped = minValue + (Math.Abs(val) % range);
			
			return mapped;
		}
	}
}
