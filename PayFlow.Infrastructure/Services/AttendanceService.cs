using PayFlow.Application.DTOs.Attendance;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Application.Interfaces.Services;
using PayFlow.Domain.Entities;
using PayFlow.Domain.Exceptions;

namespace PayFlow.Infrastructure.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepo;
    private readonly IEmployeeRepository _employeeRepo;

    public AttendanceService(
        IAttendanceRepository attendanceRepo,
        IEmployeeRepository employeeRepo)
    {
        _attendanceRepo = attendanceRepo;
        _employeeRepo = employeeRepo;
    }

    public async Task CreateOrUpdateAsync(AttendanceCreateDto dto)
    {
        var employee = await _employeeRepo.GetByIdAsync(dto.EmployeeId)
            ?? throw new NotFoundException($"Employee with ID {dto.EmployeeId} not found.");

        if (dto.DaysPresent > dto.WorkingDays)
            throw new ArgumentException("Days present cannot exceed working days.");

        if (dto.PaidLeaves < 0 || dto.PaidLeaves > dto.WorkingDays)
            throw new ArgumentException("Paid leaves cannot exceed working days.");

        var existing = await _attendanceRepo
            .GetByEmployeeMonthYearAsync(dto.EmployeeId, dto.Month, dto.Year);

        if (existing != null)
        {
            existing.WorkingDays = dto.WorkingDays;
            existing.DaysPresent = dto.DaysPresent;
            existing.PaidLeaves = dto.PaidLeaves;
            existing.UpdatedAt = DateTime.UtcNow;
            await _attendanceRepo.UpdateAsync(existing);
        }
        else
        {
            var attendance = new Attendance
            {
                EmployeeId = dto.EmployeeId,
                Month = dto.Month,
                Year = dto.Year,
                WorkingDays = dto.WorkingDays,
                DaysPresent = dto.DaysPresent,
                PaidLeaves = dto.PaidLeaves,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _attendanceRepo.AddAsync(attendance);
        }
    }

    public async Task<AttendanceResponseDto> GetByEmployeeMonthYearAsync(
        int employeeId, int month, int year)
    {
        var attendance = await _attendanceRepo
            .GetByEmployeeMonthYearAsync(employeeId, month, year)
            ?? throw new NotFoundException("Attendance record not found.");

        return MapToResponse(attendance);
    }

    public async Task<IEnumerable<AttendanceResponseDto>> GetAllByMonthYearAsync(
        int month, int year)
    {
        var records = await _attendanceRepo.GetAllByMonthYearAsync(month, year);
        return records.Select(a => MapToResponse(a));
    }

    public async Task<IEnumerable<AttendanceResponseDto>> GetByEmployeeIdAsync(int employeeId)
    {
        var records = await _attendanceRepo.GetByEmployeeIdAsync(employeeId);
        return records.Select(a => MapToResponse(a));
    }

    

    private AttendanceResponseDto MapToResponse(Attendance a)
    {
        var effectiveWD = a.WorkingDays - a.PaidLeaves;
        var unpaidLeaves = Math.Max(0, effectiveWD - a.DaysPresent);
        var perDay = effectiveWD > 0
            ? (a.Employee?.BasicPay ?? 0) / effectiveWD
            : 0;
        var deductionPreview = Math.Round(perDay * unpaidLeaves, 2);

        return new AttendanceResponseDto
        {
            AttendanceId = a.AttendanceId,
            EmployeeId = a.EmployeeId,
            EmployeeName = a.Employee?.Name ?? string.Empty,
            PhotoPath = a.Employee?.PhotoPath,
            Month = a.Month,
            Year = a.Year,
            WorkingDays = a.WorkingDays,
            DaysPresent = a.DaysPresent,
            PaidLeaves = a.PaidLeaves,
            EffectiveWorkingDays = effectiveWD,
            UnpaidLeaves = unpaidLeaves,
            Leaves = unpaidLeaves,
            DeductionPreview = deductionPreview
        };
    }
}
