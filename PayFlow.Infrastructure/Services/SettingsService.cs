using PayFlow.Application.DTOs.Settings;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Application.Interfaces.Services;
using PayFlow.Domain.Exceptions;

namespace PayFlow.Infrastructure.Services;

public class SettingsService : ISettingsService
{
    private readonly IBranchSettingsRepository _settingsRepo;
    private readonly IUserRepository _userRepo;

    public SettingsService(
        IBranchSettingsRepository settingsRepo,
        IUserRepository userRepo)
    {
        _settingsRepo = settingsRepo;
        _userRepo = userRepo;
    }

    public async Task<BranchSettingsResponseDto> GetSettingsAsync(int userId)
    {
        var settings = await _settingsRepo.GetAsync()
            ?? throw new NotFoundException("Branch settings not found.");

        string? username = null;
        string? lastLogin = null;

        if (userId > 0)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            username = user?.Username;
            lastLogin = user?.LastLoginAt?.ToString("dd MMM yyyy hh:mm tt");
        }

        return new BranchSettingsResponseDto
        {
            Id = settings.Id,
            SchoolName = settings.SchoolName,
            PrincipalName = settings.PrincipalName,
            Email = settings.Email,
            Address = settings.Address,
            Contact = settings.Contact,
            LogoPath = settings.LogoPath,
            Username = username,
            LastLogin = lastLogin
        };
    }

    public async Task UpdateSettingsAsync(BranchSettingsUpdateDto dto)
    {
        var settings = await _settingsRepo.GetAsync()
            ?? throw new NotFoundException("Branch settings not found.");

        settings.SchoolName = dto.SchoolName;
        settings.PrincipalName = dto.PrincipalName;
        settings.Email = dto.Email;
        settings.Address = dto.Address;
        settings.Contact = dto.Contact;
        settings.UpdatedAt = DateTime.UtcNow;

        await _settingsRepo.UpdateAsync(settings);
    }

    public async Task UpdateLogoAsync(int settingsId, string logoPath)
    {
        var settings = await _settingsRepo.GetAsync()
            ?? throw new NotFoundException("Branch settings not found.");

        settings.LogoPath = logoPath;
        settings.UpdatedAt = DateTime.UtcNow;

        await _settingsRepo.UpdateAsync(settings);
    }
}
