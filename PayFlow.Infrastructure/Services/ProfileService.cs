using PayFlow.Application.DTOs.Attendance;
using PayFlow.Application.DTOs.Employee;
using PayFlow.Application.DTOs.Payroll;
using PayFlow.Application.DTOs.Profile;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Application.Interfaces.Services;
using PayFlow.Domain.Exceptions;

namespace PayFlow.Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly IUserRepository _userRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IPayrollRepository _payrollRepo;
    private readonly IAttendanceRepository _attendanceRepo;

    public ProfileService(
        IUserRepository userRepo,
        IEmployeeRepository employeeRepo,
        IPayrollRepository payrollRepo,
        IAttendanceRepository attendanceRepo)
    {
        _userRepo = userRepo;
        _employeeRepo = employeeRepo;
        _payrollRepo = payrollRepo;
        _attendanceRepo = attendanceRepo;
    }

    public async Task<EmployeeResponseDto> GetProfileAsync(int userId)
    {
        var user = await _userRepo.GetByIdAsync(userId)
            ?? throw new NotFoundException("User not found.");

        if (user.EmployeeId == null)
            throw new NotFoundException("No employee record linked to this account.");

        var employee = await _employeeRepo.GetByIdAsync(user.EmployeeId.Value)
            ?? throw new NotFoundException("Employee not found.");

        return new EmployeeResponseDto
        {
            EmployeeId = employee.EmployeeId,
            Name = employee.Name,
            Designation = employee.Designation,
            Address = employee.Address,
            Contact = employee.Contact,
            Email = employee.Email,
            Aadhaar = employee.Aadhaar,
            DateOfJoining = employee.DateOfJoining,
            BasicPay = employee.BasicPay,
            ConveyanceAllowance = employee.ConveyanceAllowance,
            PhotoPath = employee.PhotoPath,
            IsActive = employee.IsActive
        };
    }

    public async Task UpdateProfileAsync(int userId, ProfileUpdateDto dto)
    {
        var user = await _userRepo.GetByIdAsync(userId)
            ?? throw new NotFoundException("User not found.");

        if (user.EmployeeId == null)
            throw new NotFoundException("No employee record linked to this account.");

        var employee = await _employeeRepo.GetByIdAsync(user.EmployeeId.Value)
            ?? throw new NotFoundException("Employee not found.");

        // Employee can only update contact info — not salary or designation
        employee.Address = dto.Address;
        employee.Contact = dto.Contact;
        employee.Email = dto.Email;
        employee.UpdatedAt = DateTime.UtcNow;

        await _employeeRepo.UpdateAsync(employee);
    }

    public async Task<IEnumerable<PayrollResponseDto>> GetMyPayslipsAsync(int userId)
    {
        var user = await _userRepo.GetByIdAsync(userId)
            ?? throw new NotFoundException("User not found.");

        if (user.EmployeeId == null)
            throw new NotFoundException("No employee record linked to this account.");

        var records = await _payrollRepo.GetByEmployeeIdAsync(user.EmployeeId.Value);

        return records.Select(p => new PayrollResponseDto
        {
            PayrollId = p.PayrollId,
            EmployeeId = p.EmployeeId,
            EmployeeName = p.Employee?.Name ?? string.Empty,
            Designation = p.Employee?.Designation ?? string.Empty,
            Month = p.Month,
            Year = p.Year,
            BasicPay = p.BasicPay,
            Allowance = p.Allowance,
            Deduction = p.Deduction,
            NetPay = p.NetPay,
            IsPaid = p.IsPaid,
            PaidDate = p.PaidDate
        });
    }

    public async Task<IEnumerable<AttendanceResponseDto>> GetMyAttendanceAsync(int userId)
    {
        var user = await _userRepo.GetByIdAsync(userId)
            ?? throw new NotFoundException("User not found.");

        if (user.EmployeeId == null)
            throw new NotFoundException("No employee record linked to this account.");

        var records = await _attendanceRepo.GetByEmployeeIdAsync(user.EmployeeId.Value);

        return records.Select(a => new AttendanceResponseDto
        {
            AttendanceId = a.AttendanceId,
            EmployeeId = a.EmployeeId,
            EmployeeName = a.Employee?.Name ?? string.Empty,
            Month = a.Month,
            Year = a.Year,
            WorkingDays = a.WorkingDays,
            DaysPresent = a.DaysPresent,
            Leaves = a.Leaves
        });
    }
}
