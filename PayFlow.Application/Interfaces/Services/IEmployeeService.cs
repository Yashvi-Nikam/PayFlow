using PayFlow.Application.DTOs.Employee;

namespace PayFlow.Application.Interfaces.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeResponseDto>> GetAllAsync();
    Task<EmployeeResponseDto> GetByIdAsync(int employeeId);
    Task AddAsync(EmployeeCreateDto dto);
    Task UpdateAsync(int employeeId, EmployeeUpdateDto dto);
    Task ActivateAsync(int employeeId);
    Task DeactivateAsync(int employeeId);
    Task CreateAccountAsync(int employeeId, CreateAccountDto dto);
    Task ResetPasswordAsync(int employeeId);
    Task UpdatePhotoAsync(int employeeId, string photoPath);

}
