using Teladoc.DataAccess.Entities;

namespace Teladoc.DataAccess.Interface;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task AddUserAsync(User user);
    Task<bool> IsEmailUniqueAsync(string email);
}