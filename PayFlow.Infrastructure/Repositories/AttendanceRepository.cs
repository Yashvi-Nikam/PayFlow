using Microsoft.EntityFrameworkCore;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Domain.Entities;
using PayFlow.Infrastructure.Persistence;

namespace PayFlow.Infrastructure.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly AppDbContext _context;

    public AttendanceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Attendance?> GetByEmployeeMonthYearAsync(int employeeId, int month, int year)
    {
        return await _context.Attendances
            .Include(a => a.Employee)
            .FirstOrDefaultAsync(a =>
                a.EmployeeId == employeeId &&
                a.Month == month &&
                a.Year == year);
    }

    public async Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _context.Attendances
            .Include(a => a.Employee)
            .Where(a => a.EmployeeId == employeeId)
            .OrderByDescending(a => a.Year)
            .ThenByDescending(a => a.Month)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetAllByMonthYearAsync(int month, int year)
    {
        return await _context.Attendances
            .Include(a => a.Employee)
            .Where(a => a.Month == month && a.Year == year)
            .OrderBy(a => a.Employee!.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Attendance attendance)
    {
        await _context.Attendances.AddAsync(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Attendance attendance)
    {
        _context.Attendances.Update(attendance);
        await _context.SaveChangesAsync();
    }
}
