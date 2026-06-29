using PayFlow.Domain.Entities;

namespace PayFlow.Application.Interfaces.Repositories;

public interface IPayrollRepository
{
    Task<Payroll?> GetByIdAsync(int payrollId);
    Task<Payroll?> GetByEmployeeMonthYearAsync(int employeeId, int month, int year);
    Task<IEnumerable<Payroll>> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<Payroll>> GetAllByMonthYearAsync(int month, int year);
    Task AddAsync(Payroll payroll);
    Task UpdateAsync(Payroll payroll);
}
