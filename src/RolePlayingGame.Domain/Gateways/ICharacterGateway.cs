using RolePlayingGame.Application.Domain.Entities;

namespace RolePlayingGame.Domain.Gateways
{
	public interface ICharacterGateway
	{
		Task<Character> CreateAsync(Character character);
		Task<Character?> GetByIdAsync(Guid id);
		Task<IEnumerable<Character>> GetAllAsync();
		Task<Character> UpdateAsync(Character character);
		Task<bool> DeleteAsync(Guid id);
	}
}
