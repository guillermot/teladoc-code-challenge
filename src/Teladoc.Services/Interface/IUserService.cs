using Teladoc.Services.Domain;

namespace Teladoc.Services.Interface;

public interface IUserService
{
    Task<UserCreationResult> CreateUserAsync(User user);
    Task<IEnumerable<User>> GetAllUsersAsync();
}