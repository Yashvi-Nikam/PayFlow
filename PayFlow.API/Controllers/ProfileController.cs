using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.Application.DTOs.Profile;
using PayFlow.Application.Interfaces.Services;
using PayFlow.Infrastructure.Services;
using System.Security.Claims;

namespace PayFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Employee")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly FileStorageService _fileStorage;
    private readonly IEmployeeService _employeeService;

    public ProfileController(
        IProfileService profileService,
        FileStorageService fileStorage,
        IEmployeeService employeeService)
    {
        _profileService = profileService;
        _fileStorage = fileStorage;
        _employeeService = employeeService;
    }

    // GET /api/profile
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetUserId();
        var result = await _profileService.GetProfileAsync(userId);
        return Ok(result);
    }

    // PUT /api/profile
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDto dto)
    {
        var userId = GetUserId();
        await _profileService.UpdateProfileAsync(userId, dto);
        return Ok(new { message = "Profile updated successfully." });
    }

    // GET /api/profile/payslips
    [HttpGet("payslips")]
    public async Task<IActionResult> GetMyPayslips()
    {
        var userId = GetUserId();
        var result = await _profileService.GetMyPayslipsAsync(userId);
        return Ok(result);
    }

    // GET /api/profile/attendance
    [HttpGet("attendance")]
    public async Task<IActionResult> GetMyAttendance()
    {
        var userId = GetUserId();
        var result = await _profileService.GetMyAttendanceAsync(userId);
        return Ok(result);
    }

    // POST /api/profile/upload-photo
    [HttpPost("upload-photo")]
    public async Task<IActionResult> UploadMyPhoto(IFormFile file)
    {
        var userId = GetUserId();

        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file provided." });

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
            return BadRequest(new { message = "Only JPG and PNG allowed." });

        var profile = await _profileService.GetProfileAsync(userId);

        using var stream = file.OpenReadStream();
        var photoPath = await _fileStorage.SaveEmployeePhotoAsync(
            profile.EmployeeId, stream, file.FileName);

        await _employeeService.UpdatePhotoAsync(profile.EmployeeId, photoPath);

        return Ok(new { message = "Photo uploaded successfully.", photoPath });
    }

    private int GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token.");
        return int.Parse(claim);
    }
}
