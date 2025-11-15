using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Domain.Enums;
using RolePlayingGame.Application.Dtos.Request;
using RolePlayingGame.Application.Dtos.Response;

namespace RolePlayingGame.Application.Mappers
{
    public static class PostCharacterMapper
    {
		public static Character ToDomain(this PostCharacterRequest request)
		{
			if (!Enum.TryParse<Job>(request.Job, ignoreCase: true, out var jobEnum))
				throw new ArgumentException($"Invalid job '{request.Job}'");

			return new Character(
				name: request.Name,
				job: jobEnum
			);
		}

		public static PostCharacterResponse ToPostResponse(this Character character)
		{
			return new PostCharacterResponse
			{
				Name = character.Name,
				Job = character.Job.ToString(),
				HealthPoints = character.MaximumHealthPoints,
				Strength = character.Strength,
				Dexterity = character.Dexterity,
				Intelligence = character.Intelligence,
				AttackModifier = character.AttackModifier,
				SpeedModifier = character.SpeedModifier
			};
		}
	}
}
