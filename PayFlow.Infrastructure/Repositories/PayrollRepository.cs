using Microsoft.EntityFrameworkCore;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Domain.Entities;
using PayFlow.Infrastructure.Persistence;

namespace PayFlow.Infrastructure.Repositories;

public class PayrollRepository : IPayrollRepository
{
    private readonly AppDbContext _context;

    public PayrollRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payroll?> GetByIdAsync(int payrollId)
    {
        return await _context.Payrolls
            .Include(p => p.Employee)
            .FirstOrDefaultAsync(p => p.PayrollId == payrollId);
    }

    public async Task<Payroll?> GetByEmployeeMonthYearAsync(
        int employeeId, int month, int year)
    {
        return await _context.Payrolls
            .Include(p => p.Employee)
            .FirstOrDefaultAsync(p =>
                p.EmployeeId == employeeId &&
                p.Month == month &&
                p.Year == year);
    }

    public async Task<IEnumerable<Payroll>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _context.Payrolls
            .Include(p => p.Employee)
            .Where(p => p.EmployeeId == employeeId)
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.Month)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payroll>> GetAllByMonthYearAsync(int month, int year)
    {
        return await _context.Payrolls
            .Include(p => p.Employee)
            .Where(p => p.Month == month && p.Year == year)
            .OrderBy(p => p.Employee!.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Payroll payroll)
    {
        await _context.Payrolls.AddAsync(payroll);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Payroll payroll)
    {
        _context.Payrolls.Update(payroll);
        await _context.SaveChangesAsync();
    }
}
