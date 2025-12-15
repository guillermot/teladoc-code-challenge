using Microsoft.EntityFrameworkCore;
using Teladoc.DataAccess.Entities;
using Teladoc.DataAccess.Interface;

namespace Teladoc.DataAccess.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    private readonly AppDbContext dbContext = dbContext;

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await this
            .dbContext
            .Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddUserAsync(User user)
    {
        this.dbContext.Users.Add(user);
        await this.dbContext.SaveChangesAsync();
    }

    public Task<bool> IsEmailUniqueAsync(string email)
    {
        return this.dbContext
            .Users
            .AnyAsync(user=> user.Email == email);
    }
}