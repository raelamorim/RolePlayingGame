using System;

namespace RolePlayingGame.Domain.Services
{
	/// <summary>
	/// Default Random provider. Uses System.Random (no seed) for production.
	/// </summary>
	public class DefaultRandomProvider : IRandomProvider
	{
		private readonly Random _rnd;

		public DefaultRandomProvider()
		{
			_rnd = new Random();
		}

		public int Next(int minValue, int maxValue) => _rnd.Next(minValue, maxValue);
	}
}
