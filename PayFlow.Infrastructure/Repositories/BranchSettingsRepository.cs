using Microsoft.EntityFrameworkCore;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Domain.Entities;
using PayFlow.Infrastructure.Persistence;

namespace PayFlow.Infrastructure.Repositories;

public class BranchSettingsRepository : IBranchSettingsRepository
{
    private readonly AppDbContext _context;

    public BranchSettingsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BranchSettings?> GetAsync()
    {
        return await _context.BranchSettings.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(BranchSettings settings)
    {
        _context.BranchSettings.Update(settings);
        await _context.SaveChangesAsync();
    }
}
