using LoginApp.Application.Users;
using Microsoft.AspNetCore.Mvc;
using LoginApp.Application.Common.Exceptions;
using LoginApp.Application;
using LoginApp.Api.Models;
using LoginApp.Application.Users.Dtos;
using LoginApp.Application.Users.RequestDtos;

namespace LoginApp.Api.Controllers.User;

[ApiController]
[Route("api/users")]
public class UsersController(
    IAuthService _authService,
    IUserQueries _queries
) : ApiControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct) =>
        ApiOk(await _queries.GetAsync(id, ct) ?? throw new NotFoundException(UserMessages.NotFound));

    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestDto request, CancellationToken ct)
    {
        var user = await _authService.RegisterAsync(request, ct);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, ApiResponse<UserDto>.SuccesResponse(user));
    }
}
