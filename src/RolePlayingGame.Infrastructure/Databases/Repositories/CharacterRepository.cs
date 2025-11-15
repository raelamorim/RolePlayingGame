using Microsoft.EntityFrameworkCore;
using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Domain.Gateways;
using RolePlayingGame.Insfrastructure.Databases.Context;

namespace RolePlayingGame.Infrastructure.Databases.Repositories
{
    public class CharacterRepository : ICharacterGateway
    {
		private readonly RolePlayingGameDbContext _context;

		public CharacterRepository(RolePlayingGameDbContext context)
		{
			_context = context;
		}

		public async Task<Character> CreateAsync(Character character)
		{
			_context.Characters.Add(character);
			await _context.SaveChangesAsync();
			return character;
		}

		public async Task<Character?> GetByIdAsync(Guid id)
		{
			return await _context.Characters.FindAsync(id);
		}

		public async Task<IEnumerable<Character>> GetAllAsync()
		{
			return await _context.Characters.AsNoTracking().ToListAsync();
		}

		public async Task<Character> UpdateAsync(Character character)
		{
			_context.Characters.Update(character);
			await _context.SaveChangesAsync();
			return character;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var entity = await _context.Characters.FindAsync(id);
			if (entity == null)
				return false;

			_context.Characters.Remove(entity);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
