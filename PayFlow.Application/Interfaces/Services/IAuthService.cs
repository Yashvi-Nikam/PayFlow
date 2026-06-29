using PayFlow.Application.DTOs.Auth;

namespace PayFlow.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
}
