using Microsoft.EntityFrameworkCore;
using RolePlayingGame.Application.Domain.Entities;
using RolePlayingGame.Application.Domain.Enums;
using RolePlayingGame.Infrastructure.Databases.Repositories;
using RolePlayingGame.Insfrastructure.Databases.Context;

namespace RolePlayingGame.UnitTest.Insfrastructure.Databases.Repositories
{
    public class CharacterRepositoryTests
    {
		private RolePlayingGameDbContext CreateInMemoryDb()
		{
			var options = new DbContextOptionsBuilder<RolePlayingGameDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // isolamento total
				.Options;

			return new RolePlayingGameDbContext(options);
		}

		private Character CreateCharacter(string name = "TestHero", Job job = Job.Warrior)
		{
			return new Character(name, job);
		}

		[Fact(DisplayName = "CreateAsync stores a character successfully")]
		public async Task CreateAsync_SavesCharacter()
		{
			var db = CreateInMemoryDb();
			var repository = new CharacterRepository(db);

			var character = CreateCharacter();

			var result = await repository.CreateAsync(character);

			Assert.NotNull(result);
			Assert.Equal(character.Id, result.Id);

			// Confirm persisted
			var fromDb = await db.Characters.FindAsync(character.Id);
			Assert.NotNull(fromDb);
		}

		[Fact(DisplayName = "GetByIdAsync returns character when found")]
		public async Task GetByIdAsync_ReturnsCharacter()
		{
			var db = CreateInMemoryDb();
			var character = CreateCharacter();
			db.Characters.Add(character);
			await db.SaveChangesAsync();

			var repository = new CharacterRepository(db);

			var result = await repository.GetByIdAsync(character.Id);

			Assert.NotNull(result);
			Assert.Equal(character.Id, result!.Id);
		}

		[Fact(DisplayName = "GetByIdAsync returns null when not found")]
		public async Task GetByIdAsync_ReturnsNull()
		{
			var db = CreateInMemoryDb();
			var repository = new CharacterRepository(db);

			var result = await repository.GetByIdAsync(Guid.NewGuid());

			Assert.Null(result);
		}

		[Fact(DisplayName = "GetAllAsync returns all characters")]
		public async Task GetAllAsync_ReturnsAll()
		{
			var db = CreateInMemoryDb();
			db.Characters.Add(CreateCharacter("C1"));
			db.Characters.Add(CreateCharacter("C2"));
			await db.SaveChangesAsync();

			var repository = new CharacterRepository(db);

			var result = await repository.GetAllAsync();

			Assert.Equal(2, result.Count());
		}

		[Fact(DisplayName = "UpdateAsync updates character successfully")]
		public async Task UpdateAsync_UpdatesCharacter()
		{
			var db = CreateInMemoryDb();
			var character = CreateCharacter();
			db.Characters.Add(character);
			await db.SaveChangesAsync();

			var repository = new CharacterRepository(db);

			// Act – mudar job
			character.ChangeJob(Job.Mage);

			var updated = await repository.UpdateAsync(character);

			Assert.NotNull(updated);
			Assert.Equal(Job.Mage, updated.Job);

			// Confirm in DB
			var fromDb = await db.Characters.FindAsync(character.Id);
			Assert.NotNull(fromDb);
			Assert.Equal(Job.Mage, fromDb!.Job);
		}

		[Fact(DisplayName = "DeleteAsync returns false when entity does not exist")]
		public async Task DeleteAsync_NotFound_ReturnsFalse()
		{
			var db = CreateInMemoryDb();
			var repository = new CharacterRepository(db);

			var result = await repository.DeleteAsync(Guid.NewGuid());

			Assert.False(result);
		}

		[Fact(DisplayName = "DeleteAsync removes entity and returns true")]
		public async Task DeleteAsync_RemovesCharacter()
		{
			var db = CreateInMemoryDb();
			var character = CreateCharacter();
			db.Characters.Add(character);
			await db.SaveChangesAsync();

			var repository = new CharacterRepository(db);

			var result = await repository.DeleteAsync(character.Id);

			Assert.True(result);

			var fromDb = await db.Characters.FindAsync(character.Id);
			Assert.Null(fromDb);
		}
	}
}
