using PayFlow.Domain.Entities;

namespace PayFlow.Application.Interfaces.Repositories;

public interface IBranchSettingsRepository
{
    Task<BranchSettings?> GetAsync();
    Task UpdateAsync(BranchSettings settings);
}
