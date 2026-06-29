using Microsoft.EntityFrameworkCore;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Domain.Entities;
using PayFlow.Infrastructure.Persistence;

namespace PayFlow.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    public async Task<User?> GetByEmployeeIdAsync(int employeeId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
    }
}
