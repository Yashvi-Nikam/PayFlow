using PayFlow.Application.DTOs.Auth;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Application.Interfaces.Services;
using PayFlow.Domain.Exceptions;

namespace PayFlow.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly JwtTokenService _jwtService;

    public AuthService(IUserRepository userRepo, JwtTokenService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepo.GetByUsernameAsync(dto.Username);

        if (user == null)
            throw new NotFoundException($"User not found with username: {dto.Username}");

        bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!isValid)
            throw new NotFoundException($"Password incorrect for user: {dto.Username}");

        user.LastLoginAt = DateTime.UtcNow;
        await _userRepo.UpdateAsync(user);

        var token = _jwtService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            Role = user.Role,
            Username = user.Username,
            IsFirstLogin = user.IsFirstLogin
        };
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _userRepo.GetByIdAsync(userId)
            ?? throw new NotFoundException("User not found.");

        // Verify current password
        bool isValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);
        if (!isValid)
            throw new NotFoundException("Current password is incorrect.");

        // Hash new password and save
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.IsFirstLogin = false;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepo.UpdateAsync(user);
    }
}
