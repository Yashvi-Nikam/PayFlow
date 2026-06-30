using PayFlow.Application.DTOs.Employee;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Application.Interfaces.Services;
using PayFlow.Domain.Entities;
using PayFlow.Domain.Exceptions;

namespace PayFlow.Infrastructure.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IUserRepository _userRepo;

    public EmployeeService(IEmployeeRepository employeeRepo, IUserRepository userRepo)
    {
        _employeeRepo = employeeRepo;
        _userRepo = userRepo;
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetAllAsync()
    {
        var employees = await _employeeRepo.GetAllAsync();
        return employees.Select(e => MapToResponse(e));
    }

    public async Task<EmployeeResponseDto> GetByIdAsync(int employeeId)
    {
        var employee = await _employeeRepo.GetByIdAsync(employeeId)
            ?? throw new NotFoundException($"Employee with ID {employeeId} not found.");
        return MapToResponse(employee);
    }

    public async Task AddAsync(EmployeeCreateDto dto)
    {
        // Check email duplicate
        if (!string.IsNullOrEmpty(dto.Email))
        {
            var existing = await _employeeRepo.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new DuplicateRecordException("An employee with this email already exists.");
        }
        // Check Aadhaar duplicate
        if (!string.IsNullOrEmpty(dto.Aadhaar))
        {
            var existing = await _employeeRepo.GetByAadhaarAsync(dto.Aadhaar);
            if (existing != null)
                throw new DuplicateRecordException("An employee with this Aadhaar number already exists.");
        }

        var employee = new Employee
        {
            Name = dto.Name,
            Designation = dto.Designation,
            Address = dto.Address,
            Contact = dto.Contact,
            Email = dto.Email,
            Aadhaar = dto.Aadhaar,
            DateOfJoining = dto.DateOfJoining,
            BasicPay = dto.BasicPay,
            ConveyanceAllowance = dto.ConveyanceAllowance,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _employeeRepo.AddAsync(employee);
    }

    public async Task UpdateAsync(int employeeId, EmployeeUpdateDto dto)
    {
        var employee = await _employeeRepo.GetByIdAsync(employeeId)
            ?? throw new NotFoundException($"Employee with ID {employeeId} not found.");

        employee.Name = dto.Name;
        employee.Designation = dto.Designation;
        employee.Address = dto.Address;
        employee.Contact = dto.Contact;
        employee.Email = dto.Email;
        employee.Aadhaar = dto.Aadhaar;
        employee.BasicPay = dto.BasicPay;
        employee.ConveyanceAllowance = dto.ConveyanceAllowance;
        employee.UpdatedAt = DateTime.UtcNow;

        await _employeeRepo.UpdateAsync(employee);
    }

    public async Task ActivateAsync(int employeeId)
    {
        var employee = await _employeeRepo.GetByIdAsync(employeeId)
            ?? throw new NotFoundException($"Employee with ID {employeeId} not found.");

        employee.IsActive = true;
        employee.UpdatedAt = DateTime.UtcNow;

        await _employeeRepo.UpdateAsync(employee);
    }

    public async Task DeactivateAsync(int employeeId)
    {
        var employee = await _employeeRepo.GetByIdAsync(employeeId)
            ?? throw new NotFoundException($"Employee with ID {employeeId} not found.");

        employee.IsActive = false;
        employee.UpdatedAt = DateTime.UtcNow;

        await _employeeRepo.UpdateAsync(employee);
    }

    public async Task CreateAccountAsync(int employeeId, CreateAccountDto dto)
    {
        // Check employee exists
        var employee = await _employeeRepo.GetByIdAsync(employeeId)
            ?? throw new NotFoundException($"Employee with ID {employeeId} not found.");

        // Check username not already taken
        var existingUser = await _userRepo.GetByUsernameAsync(dto.Username);
        if (existingUser != null)
            throw new DuplicateRecordException("Username already exists.");

        // Create account with temporary password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Welcome@123");

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = hashedPassword,
            Role = "Employee",
            EmployeeId = employeeId,
            IsFirstLogin = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepo.AddAsync(user);
    }

    private EmployeeResponseDto MapToResponse(Employee e) => new()
{
    EmployeeId          = e.EmployeeId,
    Name                = e.Name,
    Designation         = e.Designation,
    Address             = e.Address,
    Contact             = e.Contact,
    Email               = e.Email,
    Aadhaar             = e.Aadhaar,
    DateOfJoining       = e.DateOfJoining,
    BasicPay            = e.BasicPay,
    ConveyanceAllowance = e.ConveyanceAllowance,
    PhotoPath           = e.PhotoPath,
    IsActive            = e.IsActive,
    HasAccount          = e.User != null
};
    public async Task ResetPasswordAsync(int employeeId)
    {
        var user = await _userRepo.GetByEmployeeIdAsync(employeeId)
            ?? throw new NotFoundException("No account found for this employee.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Welcome@123");
        user.IsFirstLogin = true;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepo.UpdateAsync(user);
    }
    public async Task UpdatePhotoAsync(int employeeId, string photoPath)
    {
        var employee = await _employeeRepo.GetByIdAsync(employeeId)
            ?? throw new NotFoundException($"Employee not found.");

        employee.PhotoPath = photoPath;
        employee.UpdatedAt = DateTime.UtcNow;

        await _employeeRepo.UpdateAsync(employee);
    }
}
