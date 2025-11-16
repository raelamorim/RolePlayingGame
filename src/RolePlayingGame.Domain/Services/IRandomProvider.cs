namespace RolePlayingGame.Domain.Services
{
	/// <summary>
	/// Small abstraction over random number generation so tests can inject deterministic behaviour.
	/// </summary>
	public interface IRandomProvider
	{
		int Next(int minValue, int maxValue);
	}
}
