namespace RolePlayingGame.Application.Dtos.Response
{
    public class GetCharacterListResponse
    {
		public Guid Id { get; set; }
		public string Name { get; set; } = "";
		public string Job { get; set; } = "";
		public string Status { get; set; } = "";
	}
}
