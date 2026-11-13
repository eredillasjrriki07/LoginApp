using LoginApp.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginApp.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ApiOk<T>(T data, string? message = null) =>
        Ok(ApiResponse<T>.SuccesResponse(data, message));

    protected IActionResult ApiBadRequest(string message, Dictionary<string, string[]>? errors = null) =>
        BadRequest(ApiResponse<object>.ErrorResponse(message, errors));
}
