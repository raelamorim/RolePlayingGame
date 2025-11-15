using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Dtos.Response;

namespace RolePlayingGame.Application.Mappers
{
    public static class GetCharacterListMapper
    {
		public static GetCharacterListResponse ToListResponse(this Character character)
		{
			return new GetCharacterListResponse
			{
				Id = character.Id,
				Name = character.Name,
				Job = character.Job.ToString(),
				Status = character.Status.ToString()
			};
		}
	}
}
