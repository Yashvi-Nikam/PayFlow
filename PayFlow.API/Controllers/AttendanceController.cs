using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.Application.DTOs.Attendance;
using PayFlow.Application.Interfaces.Services;

namespace PayFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    // POST /api/attendance
    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate([FromBody] AttendanceCreateDto dto)
    {
        await _attendanceService.CreateOrUpdateAsync(dto);
        return Ok(new { message = "Attendance saved successfully." });
    }

    // GET /api/attendance/{employeeId}/{year}/{month}
    [HttpGet("{employeeId}/{year}/{month}")]
    public async Task<IActionResult> GetByEmployeeMonthYear(
        int employeeId, int year, int month)
    {
        var result = await _attendanceService
            .GetByEmployeeMonthYearAsync(employeeId, month, year);
        return Ok(result);
    }

    // GET /api/attendance/month/{year}/{month}
    [HttpGet("month/{year}/{month}")]
    public async Task<IActionResult> GetAllByMonthYear(int year, int month)
    {
        var result = await _attendanceService.GetAllByMonthYearAsync(month, year);
        return Ok(result);
    }

    // GET /api/attendance/employee/{employeeId}
    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(int employeeId)
    {
        var result = await _attendanceService.GetByEmployeeIdAsync(employeeId);
        return Ok(result);
    }
}
