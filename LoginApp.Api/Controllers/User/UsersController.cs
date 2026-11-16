using LoginApp.Application.Users;
using Microsoft.AspNetCore.Mvc;
using LoginApp.Application.Common.Exceptions;
using LoginApp.Application;
using LoginApp.Api.Models;
using LoginApp.Application.Users.Dtos;
using LoginApp.Application.Users.RequestDtos;
using Microsoft.AspNetCore.Authorization;
using LoginApp.Api.Extensions;
using LoginApp.Application.Authentication;
using LoginApp.Application.Authentication.RequestDtos;
using LoginApp.Api.Security;
using LoginApp.Domain.Security;

namespace LoginApp.Api.Controllers.User;

[ApiController]
[Route("api/users")]
public class UsersController(
    IAuthService _authService,
    IUserQueries _queries
) : ApiControllerBase
{
    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        if (User.GetUserId() != id) throw new ForbiddenException();
        return ApiOk(await _queries.GetAsync(id, ct) ?? throw new NotFoundException(UserMessages.NotFound));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken ct)
        => ApiOk(await _authService.LoginAsync(request, ct));

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestDto request, CancellationToken ct)
    {
        var user = await _authService.RegisterAsync(request, ct);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, ApiResponse<UserDto>.SuccesResponse(user));
    }

    [HasPermission(Permissions.UsersUpdate)]
    [HttpPut("update/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequestDto request, CancellationToken ct) =>
        ApiOk(await _authService.UpdateAsync(id, request, ct));

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(string refreshToken, CancellationToken ct) =>
        ApiOk(await _authService.RefreshAsync(refreshToken, ct));

    [HttpPost("logout")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(string refreshToken, CancellationToken ct)
    {
        await _authService.LogoutAsync(refreshToken, ct);
        return NoContent();
    }
}
