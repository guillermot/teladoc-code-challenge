using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Teladoc.Services.Domain;
using Teladoc.Services.Interface;
using Teladoc.WebApi.Controllers.V1;
using Teladoc.WebApi.Dto.v1;

namespace Teladoc.Tests.WebApi;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _controller = new UsersController(_userService.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsOkWithUsers()
    {
        var users = new List<User> { new() { FirstName = "John" } };
        var dtos = new List<UserResponseDto> { new() { FirstName = "John" } };
        
        _userService.
            Setup(s => s.GetAllUsersAsync())
            .ReturnsAsync(users);
        
        _mapper
            .Setup(m => m.Map<IEnumerable<UserResponseDto>>(users))
            .Returns(dtos);

        var result = await _controller.GetUserAsync();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CreateUserAsync_NullRequest_ReturnsBadRequest()
    {
        var result = await _controller.CreateUserAsync(null);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateUserAsync_ValidationFails_ReturnsBadRequestWithErrors()
    {
        var dto = new CreateUserDto { FirstName = "John" };
        var user = new User();
        var errors = new[] { "Email is required" };
        
        _mapper
            .Setup(m => m.Map<User>(dto))
            .Returns(user);
        
        _userService
            .Setup(s => s.CreateUserAsync(user))
            .ReturnsAsync(new UserCreationResult { Success = false, Errors = errors });

        var result = await _controller.CreateUserAsync(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errors, badRequest.Value);
    }

    [Fact]
    public async Task CreateUserAsync_Success_ReturnsOk()
    {
        var dto = new CreateUserDto { FirstName = "John" };
        var user = new User();
        
        _mapper
            .Setup(m => m.Map<User>(dto))
            .Returns(user);
        
        _userService
            .Setup(s => s.CreateUserAsync(user))
            .ReturnsAsync(new UserCreationResult { Success = true });

        var result = await _controller.CreateUserAsync(dto);

        Assert.IsType<OkResult>(result);
    }
}
