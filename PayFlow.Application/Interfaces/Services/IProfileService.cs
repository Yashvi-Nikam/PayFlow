using PayFlow.Application.DTOs.Attendance;
using PayFlow.Application.DTOs.Employee;
using PayFlow.Application.DTOs.Payroll;
using PayFlow.Application.DTOs.Profile;

namespace PayFlow.Application.Interfaces.Services;

public interface IProfileService
{
    Task<EmployeeResponseDto> GetProfileAsync(int userId);
    Task UpdateProfileAsync(int userId, ProfileUpdateDto dto);
    Task<IEnumerable<PayrollResponseDto>> GetMyPayslipsAsync(int userId);
    Task<IEnumerable<AttendanceResponseDto>> GetMyAttendanceAsync(int userId);
}
