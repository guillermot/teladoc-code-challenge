using AutoMapper;
using Moq;
using Teladoc.DataAccess.Interface;
using Teladoc.Services.Domain;
using Teladoc.Services.Services;
using UserEntity = Teladoc.DataAccess.Entities.User;

namespace Teladoc.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly UserService _service;

    public UserServiceTests()
    {
        _service = new UserService(_repository.Object, _mapper.Object);
    }

    private static User CreateValidUser() => new()
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "john@example.com",
        DateOfBirth = DateTime.Today.AddYears(-25)
    };

    [Fact]
    public async Task GetAllUsersAsync_ReturnsMappedUsers()
    {
        var entities = new List<UserEntity> { new() };
        var users = new List<User> { new() };
        _repository.Setup(r => r.GetUsersAsync()).ReturnsAsync(entities);
        _mapper.Setup(m => m.Map<IEnumerable<User>>(entities)).Returns(users);

        var result = await _service.GetAllUsersAsync();

        Assert.Equal(users, result);
    }

    [Fact]
    public async Task CreateUserAsync_ValidUser_ReturnsSuccess()
    {
        var user = CreateValidUser();
        _repository
            .Setup(r => r.IsEmailUniqueAsync(user.Email))
            .ReturnsAsync(false);
        _mapper
            .Setup(m => m.Map<UserEntity>(user))
            .Returns(new UserEntity());

        var result = await _service.CreateUserAsync(user);

        Assert.True(result.Success);
        _repository.Verify(r => r.AddUserAsync(It.IsAny<UserEntity>()), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_Underage_ReturnsError()
    {
        var user = CreateValidUser();
        user.DateOfBirth = DateTime.Today.AddYears(-17);

        var result = await _service.CreateUserAsync(user);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("18 years old"));
    }

    [Fact]
    public async Task CreateUserAsync_DuplicateEmail_ReturnsError()
    {
        var user = CreateValidUser();
        _repository.Setup(r => r.IsEmailUniqueAsync(user.Email)).ReturnsAsync(true);

        var result = await _service.CreateUserAsync(user);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("Email must be unique"));
    }

    [Fact]
    public async Task CreateUserAsync_MissingRequiredFields_ReturnsAllErrors()
    {
        var user = new User { DateOfBirth = DateTime.Today.AddYears(-25) };

        var result = await _service.CreateUserAsync(user);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("FirstName"));
        Assert.Contains(result.Errors, e => e.Contains("LastName"));
        Assert.Contains(result.Errors, e => e.Contains("Email"));
    }
}
