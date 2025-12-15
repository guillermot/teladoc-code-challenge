using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Teladoc.Services.Domain;
using Teladoc.Services.Interface;
using Teladoc.WebApi.Dto.v1;

namespace Teladoc.WebApi.Controllers.V1;

[ApiController]
[Route("v1/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService userService;
    private readonly IMapper mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        this.userService = userService;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult> GetUserAsync()
    {
        var users = await userService.GetAllUsersAsync();
        var usersResponse = this.mapper.Map<IEnumerable<UserResponseDto>>(users);
        return Ok(usersResponse);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateUserAsync([FromBody] CreateUserDto? userRequest)
    {
        if (userRequest is null)
            return BadRequest();

        var user = this.mapper.Map<User>(userRequest);
        var result = await this.userService.CreateUserAsync(user);
        
        if(!result.Success)
            return BadRequest(result.Errors);

        return Ok();
    }
}