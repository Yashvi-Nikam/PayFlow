using PayFlow.Domain.Entities;

namespace PayFlow.Application.Interfaces.Repositories;

public interface IAttendanceRepository
{
    Task<Attendance?> GetByEmployeeMonthYearAsync(int employeeId, int month, int year);
    Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<Attendance>> GetAllByMonthYearAsync(int month, int year);
    Task AddAsync(Attendance attendance);
    Task UpdateAsync(Attendance attendance);
}
