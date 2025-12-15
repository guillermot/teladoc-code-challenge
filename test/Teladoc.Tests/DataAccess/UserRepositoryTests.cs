using Microsoft.EntityFrameworkCore;
using Teladoc.DataAccess;
using Teladoc.DataAccess.Entities;
using Teladoc.DataAccess.Repositories;

namespace Teladoc.Tests.DataAccess;

public class UserRepositoryTests : IDisposable
{
    private readonly AppDbContext dbContext;
    private readonly UserRepository repository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        dbContext = new AppDbContext(options);
        repository = new UserRepository(dbContext);
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }

    [Fact]
    public async Task AddUserAsync_ShouldIncreaseUserCount()
    {
        var initialCount = (await repository.GetUsersAsync()).Count();

        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        await repository.AddUserAsync(user);

        var users = await repository.GetUsersAsync();
        Assert.Equal(initialCount + 1, users.Count());
        Assert.Contains(users, u => u.Email == user.Email);
    }

    [Fact]
    public async Task GetUsersAsync_ShouldReturnAllUsers()
    {
        var user1 = new User
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@example.com",
            DateOfBirth = new DateTime(1985, 5, 15)
        };
        var user2 = new User
        {
            FirstName = "Bob",
            LastName = "Johnson",
            Email = "bob@example.com",
            DateOfBirth = new DateTime(1992, 8, 20)
        };

        await repository.AddUserAsync(user1);
        await repository.AddUserAsync(user2);

        var users = await repository.GetUsersAsync();

        Assert.Equal(2, users.Count());
    }

    [Fact]
    public async Task IsEmailUniqueAsync_ShouldReturnTrue_WhenEmailExists()
    {
        var user = new User
        {
            FirstName = "Alice",
            LastName = "Brown",
            Email = "alice@example.com",
            DateOfBirth = new DateTime(1988, 3, 10)
        };
        await repository.AddUserAsync(user);

        var exists = await repository.IsEmailUniqueAsync("alice@example.com");

        Assert.True(exists);
    }

    [Fact]
    public async Task IsEmailUniqueAsync_ShouldReturnFalse_WhenEmailDoesNotExist()
    {
        var exists = await repository.IsEmailUniqueAsync("nonexistent@example.com");

        Assert.False(exists);
    }
}
