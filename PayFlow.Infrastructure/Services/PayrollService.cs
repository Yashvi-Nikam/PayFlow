using PayFlow.Application.DTOs.Payroll;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Application.Interfaces.Services;
using PayFlow.Domain.Entities;
using PayFlow.Domain.Exceptions;

namespace PayFlow.Infrastructure.Services;

public class PayrollService : IPayrollService
{
    private readonly IPayrollRepository _payrollRepo;
    private readonly IAttendanceRepository _attendanceRepo;
    private readonly IEmployeeRepository _employeeRepo;

    public PayrollService(
        IPayrollRepository payrollRepo,
        IAttendanceRepository attendanceRepo,
        IEmployeeRepository employeeRepo)
    {
        _payrollRepo = payrollRepo;
        _attendanceRepo = attendanceRepo;
        _employeeRepo = employeeRepo;
    }

    public async Task GenerateAsync(PayrollGenerateDto dto)
    {
        var employee = await _employeeRepo.GetByIdAsync(dto.EmployeeId)
            ?? throw new NotFoundException($"Employee with ID {dto.EmployeeId} not found.");

        var attendance = await _attendanceRepo
            .GetByEmployeeMonthYearAsync(dto.EmployeeId, dto.Month, dto.Year)
            ?? throw new NotFoundException(
                "Attendance not found for this month. Please enter attendance first.");

        var existing = await _payrollRepo
            .GetByEmployeeMonthYearAsync(dto.EmployeeId, dto.Month, dto.Year);
        if (existing != null)
            throw new DuplicateRecordException(
                "Payroll already generated for this employee this month.");

        // Auto calculate deduction based on leaves
        var leaveDays = attendance.WorkingDays - attendance.DaysPresent;
        var perDaySalary = employee.BasicPay / attendance.WorkingDays;
        var autoDeduction = dto.Deduction > 0
            ? dto.Deduction
            : Math.Round(perDaySalary * leaveDays, 2);

        var netPay = employee.BasicPay + employee.ConveyanceAllowance
                     + dto.Incentive - autoDeduction;

        var payroll = new Payroll
        {
            EmployeeId = dto.EmployeeId,
            Month = dto.Month,
            Year = dto.Year,
            BasicPay = employee.BasicPay,
            Allowance = employee.ConveyanceAllowance,
            Incentive = dto.Incentive,
            Deduction = autoDeduction,
            NetPay = netPay < 0 ? 0 : netPay,
            IsPaid = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _payrollRepo.AddAsync(payroll);
    }

    public async Task UpdateDeductionAsync(int payrollId, PayrollUpdateDto dto)
    {
        var payroll = await _payrollRepo.GetByIdAsync(payrollId)
            ?? throw new NotFoundException("Payroll not found.");

        if (payroll.IsPaid)
            throw new PayrollLockedException("Payroll is already paid and cannot be modified.");

        payroll.Deduction = dto.Deduction;
        payroll.Incentive = dto.Incentive;
        payroll.NetPay = payroll.BasicPay + payroll.Allowance
                            + dto.Incentive - dto.Deduction;
        payroll.NetPay = payroll.NetPay < 0 ? 0 : payroll.NetPay;
        payroll.UpdatedAt = DateTime.UtcNow;

        await _payrollRepo.UpdateAsync(payroll);
    }

    public async Task RecalculateAsync(int payrollId)
    {
        var payroll = await _payrollRepo.GetByIdAsync(payrollId)
            ?? throw new NotFoundException("Payroll not found.");

        if (payroll.IsPaid)
            throw new PayrollLockedException("Cannot recalculate a paid payroll.");

        var employee = await _employeeRepo.GetByIdAsync(payroll.EmployeeId)
            ?? throw new NotFoundException("Employee not found.");

        var attendance = await _attendanceRepo
            .GetByEmployeeMonthYearAsync(payroll.EmployeeId, payroll.Month, payroll.Year);

        payroll.BasicPay = employee.BasicPay;
        payroll.Allowance = employee.ConveyanceAllowance;

        if (attendance != null)
        {
            var effectiveWD = attendance.WorkingDays - attendance.PaidLeaves;
            var unpaidLeaves = Math.Max(0, effectiveWD - attendance.DaysPresent);
            var perDay = effectiveWD > 0
                ? employee.BasicPay / effectiveWD
                : 0;
            payroll.Deduction = Math.Round(perDay * unpaidLeaves, 2);
        }

        payroll.NetPay = payroll.BasicPay + payroll.Allowance
                            + payroll.Incentive - payroll.Deduction;
        payroll.NetPay = payroll.NetPay < 0 ? 0 : payroll.NetPay;
        payroll.UpdatedAt = DateTime.UtcNow;

        await _payrollRepo.UpdateAsync(payroll);
    }

    private PayrollResponseDto MapToResponse(Payroll p) => new()
    {
        PayrollId = p.PayrollId,
        EmployeeId = p.EmployeeId,
        EmployeeName = p.Employee?.Name ?? string.Empty,
        Designation = p.Employee?.Designation ?? string.Empty,
        PhotoPath = p.Employee?.PhotoPath,
        Month = p.Month,
        Year = p.Year,
        BasicPay = p.BasicPay,
        Allowance = p.Allowance,
        Incentive = p.Incentive,
        Deduction = p.Deduction,
        NetPay = p.NetPay,
        IsPaid = p.IsPaid,
        PaidDate = p.PaidDate
    };

    public async Task MarkAsPaidAsync(int payrollId)
    {
        var payroll = await _payrollRepo.GetByIdAsync(payrollId)
            ?? throw new NotFoundException("Payroll not found.");

        // Rule: Already paid = locked
        if (payroll.IsPaid)
            throw new PayrollLockedException("Payroll is already marked as paid.");

        payroll.IsPaid = true;
        payroll.PaidDate = DateOnly.FromDateTime(DateTime.UtcNow);
        payroll.UpdatedAt = DateTime.UtcNow;

        await _payrollRepo.UpdateAsync(payroll);
    }

    public async Task<PayrollResponseDto> GetByIdAsync(int payrollId)
    {
        var payroll = await _payrollRepo.GetByIdAsync(payrollId)
            ?? throw new NotFoundException("Payroll not found.");
        return MapToResponse(payroll);
    }

    public async Task<IEnumerable<PayrollResponseDto>> GetAllByMonthYearAsync(
        int month, int year)
    {
        var records = await _payrollRepo.GetAllByMonthYearAsync(month, year);
        return records.Select(p => MapToResponse(p));
    }

    public async Task<IEnumerable<PayrollResponseDto>> GetByEmployeeIdAsync(int employeeId)
    {
        var records = await _payrollRepo.GetByEmployeeIdAsync(employeeId);
        return records.Select(p => MapToResponse(p));
    }

}
