namespace RolePlayingGame.Application.Exceptions
{
	public class PlayerNotFoundApplicationException : Exception
	{
		public PlayerNotFoundApplicationException(Guid playerId)
			: base($"Player with ID '{playerId}' not found (Application Layer).")
		{ }
	}
}
