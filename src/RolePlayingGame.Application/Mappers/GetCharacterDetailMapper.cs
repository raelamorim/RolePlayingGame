
using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Dtos.Response;

namespace RolePlayingGame.Application.Mappers
{
    public static class GetCharacterDetailMapper
    {
		public static GetCharacterDetailResponse ToDetailResponse(this Character character)
		{
			return new GetCharacterDetailResponse
			{
				Id = character.Id,
				Name = character.Name,
				Job = character.Job.ToString(),
				CurrentHealthPoints = character.CurrentHealthPoints,
				MaximumHealthPoints = character.MaximumHealthPoints,
				Strength = character.Strength,
				Dexterity = character.Dexterity,
				Intelligence = character.Intelligence,
				AttackModifier = character.AttackModifier,
				SpeedModifier = character.SpeedModifier
			};
		}
	}
}
