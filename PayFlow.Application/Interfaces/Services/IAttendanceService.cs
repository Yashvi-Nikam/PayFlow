using PayFlow.Application.DTOs.Attendance;

namespace PayFlow.Application.Interfaces.Services;

public interface IAttendanceService
{
    Task CreateOrUpdateAsync(AttendanceCreateDto dto);
    Task<AttendanceResponseDto> GetByEmployeeMonthYearAsync(int employeeId, int month, int year);
    Task<IEnumerable<AttendanceResponseDto>> GetAllByMonthYearAsync(int month, int year);
    Task<IEnumerable<AttendanceResponseDto>> GetByEmployeeIdAsync(int employeeId);
}
