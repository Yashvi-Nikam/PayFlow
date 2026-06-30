using Microsoft.EntityFrameworkCore;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Domain.Entities;
using PayFlow.Infrastructure.Persistence;

namespace PayFlow.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.User)
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int employeeId)
    {
        return await _context.Employees
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
    }

    public async Task AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<Employee?> GetByAadhaarAsync(string aadhaar)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Aadhaar == aadhaar);
    }
}
