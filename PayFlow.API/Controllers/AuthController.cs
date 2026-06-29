using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.Application.DTOs.Auth;
using PayFlow.Application.Interfaces.Services;
using System.Security.Claims;

namespace PayFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST /api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }

    // POST /api/auth/change-password
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();

        int userId = int.Parse(userIdClaim);
        await _authService.ChangePasswordAsync(userId, dto);
        return Ok(new { message = "Password changed successfully." });
    }

    // TEMPORARY - DELETE AFTER USE
    [HttpGet("generate-hash")]
    [AllowAnonymous]
    public IActionResult GenerateHash()
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
        return Ok(new { hash });
    }
}
