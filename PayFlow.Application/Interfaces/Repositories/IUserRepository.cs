using PayFlow.Domain.Entities;

namespace PayFlow.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int userId);
    Task UpdateAsync(User user);
    Task AddAsync(User user);
    Task<User?> GetByEmployeeIdAsync(int employeeId);
}