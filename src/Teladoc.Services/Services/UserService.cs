using AutoMapper;
using Teladoc.DataAccess.Interface;
using Teladoc.Services.Domain;
using Teladoc.Services.Interface;

using UserEntity = Teladoc.DataAccess.Entities.User;

namespace Teladoc.Services.Services;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private const int MiniumAge = 18;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }
    
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = await this.userRepository.GetUsersAsync();
        return this.mapper.Map<IEnumerable<User>>(users);
    }
    
    public async Task<UserCreationResult> CreateUserAsync(User user)
    {
        UserCreationResult result = new ();
        (result.Success, result.Errors) = await this.ValidateUser(user);

        if (!result.Success)
            return result;
        
        var entity = this.mapper.Map<UserEntity>(user);
        await this.userRepository.AddUserAsync(entity);
        
        return result;
    }

    private async Task<(bool isValid, string[] errors)> ValidateUser(User user)
    {
        bool isValid = true;
        List<string> errors = new();
        
        if(user.Age < MiniumAge)
        {
            isValid = false;
            errors.Add($"User must be at least {MiniumAge} years old.");
        }

        if (await this.userRepository.IsEmailUniqueAsync(user.Email))
        {
            isValid = false;
            errors.Add("Email must be unique.");
        }
        
        if (!ValidateRequiredFields(user, errors))
        {
            isValid = false;
        }

        return (isValid, errors.ToArray());
    }
    
    private bool ValidateRequiredFields(User user, List<string> errors)
    {
        return ValidateField(nameof(user.FirstName), user.FirstName, errors) &
               ValidateField(nameof(user.LastName), user.LastName, errors) &
               ValidateField(nameof(user.Email), user.Email, errors) &
               ValidateField(nameof(user.DateOfBirth), user.DateOfBirth, errors);
    }

    private bool ValidateField<T>(string fieldName, T value, List<string> errors)
    {
        if (value != null && !value.Equals(default(T)))
        {
            return true;
        }
        else
        {
            errors.Add($"{fieldName} is a required field");
            return false;
        }
    }
}