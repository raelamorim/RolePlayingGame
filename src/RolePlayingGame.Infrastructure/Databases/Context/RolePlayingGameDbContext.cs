using Microsoft.EntityFrameworkCore;
using RolePlayingGame.Application.Domain.Entities;

namespace RolePlayingGame.Insfrastructure.Databases.Context
{
    public class RolePlayingGameDbContext : DbContext
    {
		public RolePlayingGameDbContext(DbContextOptions<RolePlayingGameDbContext> options) : base(options)
		{
		}

		public DbSet<Character> Characters => Set<Character>();
	}
}
