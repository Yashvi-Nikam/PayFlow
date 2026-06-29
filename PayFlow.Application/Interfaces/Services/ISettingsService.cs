using PayFlow.Application.DTOs.Settings;

namespace PayFlow.Application.Interfaces.Services;

public interface ISettingsService
{
    Task<BranchSettingsResponseDto> GetSettingsAsync(int userId);
    Task UpdateSettingsAsync(BranchSettingsUpdateDto dto);
    Task UpdateLogoAsync(int settingsId, string logoPath);
}
