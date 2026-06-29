using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.Application.DTOs.Payroll;
using PayFlow.Application.Interfaces.Services;

namespace PayFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PayrollController : ControllerBase
{
    private readonly IPayrollService _payrollService;

    public PayrollController(IPayrollService payrollService)
    {
        _payrollService = payrollService;
    }

    // POST /api/payroll/generate
    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] PayrollGenerateDto dto)
    {
        await _payrollService.GenerateAsync(dto);
        return Ok(new { message = "Payroll generated successfully." });
    }

    // GET /api/payroll/by-month/{year}/{month}
    [HttpGet("by-month/{year}/{month}")]
    public async Task<IActionResult> GetAllByMonthYear(int year, int month)
    {
        var result = await _payrollService.GetAllByMonthYearAsync(month, year);
        return Ok(result);
    }

    // GET /api/payroll/by-employee/{employeeId}
    [HttpGet("by-employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(int employeeId)
    {
        var result = await _payrollService.GetByEmployeeIdAsync(employeeId);
        return Ok(result);
    }

    // GET /api/payroll/{id}   ← LAST
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _payrollService.GetByIdAsync(id);
        return Ok(result);
    }

    // PUT /api/payroll/{id}/deduction
    [HttpPut("{id}/deduction")]
    public async Task<IActionResult> UpdateDeduction(
        int id, [FromBody] PayrollUpdateDto dto)
    {
        await _payrollService.UpdateDeductionAsync(id, dto);
        return Ok(new { message = "Deduction updated successfully." });
    }

    // PATCH /api/payroll/{id}/mark-paid
    [HttpPatch("{id}/mark-paid")]
    public async Task<IActionResult> MarkAsPaid(int id)
    {
        await _payrollService.MarkAsPaidAsync(id);
        return Ok(new { message = "Payroll marked as paid." });
    }

    // PATCH /api/payroll/{id}/recalculate
    [HttpPatch("{id}/recalculate")]
    public async Task<IActionResult> Recalculate(int id)
    {
        await _payrollService.RecalculateAsync(id);
        return Ok(new { message = "Payroll recalculated with latest salary." });
    }
}
