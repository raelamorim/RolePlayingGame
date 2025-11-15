using RolePlayingGame.Application.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RolePlayingGame.Application.Dtos.Request
{
    public class PostCharacterRequest
    {
		[Required]
		[RegularExpression("^[A-Za-z_]{4,15}$",
			ErrorMessage = "Name must contain letters or underscores and be 4–15 characters long.")]
		public string Name { get; set; } = string.Empty;

		[Required]
		[EnumDataType(typeof(Job),
			ErrorMessage = "Job must be one of: Warrior, Thief, Mage.")]
		public string Job { get; set; } = string.Empty;
	}
}
