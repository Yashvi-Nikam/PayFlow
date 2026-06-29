using PayFlow.Domain.Entities;

namespace PayFlow.Application.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int employeeId);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task<Employee?> GetByEmailAsync(string email);
    Task<Employee?> GetByAadhaarAsync(string aadhaar);
}
