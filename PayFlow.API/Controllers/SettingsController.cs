using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.Application.DTOs.Settings;
using PayFlow.Application.Interfaces.Services;
using PayFlow.Domain.Exceptions;
using PayFlow.Infrastructure.Services;
using System.Security.Claims;

namespace PayFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;
    private readonly FileStorageService _fileStorage;
    

    public SettingsController(
        ISettingsService settingsService,
        FileStorageService fileStorage)
    {
        _settingsService = settingsService;
        _fileStorage = fileStorage;
    }

    // GET /api/settings
    [HttpGet]
    [AllowAnonymous]
    
    public async Task<IActionResult> GetSettings()
    {
        var settings = await _settingsService.GetSettingsAsync(0);
        return Ok(settings);
    }

    // PUT /api/settings
    [HttpPut]
    public async Task<IActionResult> UpdateSettings([FromBody] BranchSettingsUpdateDto dto)
    {
        await _settingsService.UpdateSettingsAsync(dto);
        return Ok(new { message = "Settings updated successfully." });
    }

    // POST /api/settings/upload-logo
    [HttpPost("upload-logo")]
    public async Task<IActionResult> UploadLogo(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file provided." });

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".svg" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
            return BadRequest(new { message = "Only JPG, PNG, SVG allowed." });

        if (file.Length > 2 * 1024 * 1024)
            return BadRequest(new { message = "File size must be under 2MB." });

        using var stream = file.OpenReadStream();
        var logoPath = await _fileStorage.SaveLogoAsync(stream, file.FileName);
        await _settingsService.UpdateLogoAsync(1, logoPath);

        return Ok(new { message = "Logo uploaded successfully.", logoPath });
    }

    

    private int GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token.");
        return int.Parse(claim);
    }
}
