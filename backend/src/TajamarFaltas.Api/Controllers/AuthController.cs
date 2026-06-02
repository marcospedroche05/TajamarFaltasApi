using Microsoft.AspNetCore.Mvc;
using TajamarFaltas.Application.Auth;
using TajamarFaltas.Application.Auth.Models;

namespace TajamarFaltas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.AuthenticateAsync(request, cancellationToken);
        if (result is null) return Unauthorized();
        return Ok(result);
    }
}
