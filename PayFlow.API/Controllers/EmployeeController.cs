using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.Application.DTOs.Employee;
using PayFlow.Infrastructure.Services;
using PayFlow.Application.Interfaces.Services;

namespace PayFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly FileStorageService _fileStorage;


    public EmployeeController(IEmployeeService employeeService, FileStorageService fileStorage)
    {
        _employeeService = employeeService;
        _fileStorage = fileStorage;
    }

    // GET /api/employee
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _employeeService.GetAllAsync();
        return Ok(result);
    }

    // GET /api/employee/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _employeeService.GetByIdAsync(id);
        return Ok(result);
    }

    // POST /api/employee
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateDto dto)
    {
        await _employeeService.AddAsync(dto);
        return Ok(new { message = "Employee created successfully." });
    }

    // PUT /api/employee/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto dto)
    {
        await _employeeService.UpdateAsync(id, dto);
        return Ok(new { message = "Employee updated successfully." });
    }

    // PATCH /api/employee/{id}/activate
    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> Activate(int id)
    {
        await _employeeService.ActivateAsync(id);
        return Ok(new { message = "Employee activated." });
    }

    // PATCH /api/employee/{id}/deactivate
    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(int id)
    {
        await _employeeService.DeactivateAsync(id);
        return Ok(new { message = "Employee deactivated." });
    }

    // POST /api/employee/{id}/create-account
    [HttpPost("{id}/create-account")]
    public async Task<IActionResult> CreateAccount(int id, [FromBody] CreateAccountDto dto)
    {
        await _employeeService.CreateAccountAsync(id, dto);
        return Ok(new
        {
            message = "Account created successfully.",
            temporaryPassword = "Welcome@123"
        });
    }
    // PATCH /api/employee/{id}/reset-password
    [HttpPatch("{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(int id)
    {
        await _employeeService.ResetPasswordAsync(id);
        return Ok(new
        {
            message = "Password reset successfully.",
            temporaryPassword = "Welcome@123"
        });
    }

    [HttpPost("{id}/upload-photo")]
    public async Task<IActionResult> UploadEmployeePhoto(int id, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file provided." });

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
            return BadRequest(new { message = "Only JPG and PNG allowed." });

        using var stream = file.OpenReadStream();
        var photoPath = await _fileStorage.SaveEmployeePhotoAsync(id, stream, file.FileName);

        // ← ADD THIS: update PhotoPath in Employee table
        await _employeeService.UpdatePhotoAsync(id, photoPath);

        return Ok(new { message = "Photo uploaded successfully.", photoPath });
    }
}
