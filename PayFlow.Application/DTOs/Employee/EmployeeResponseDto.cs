namespace PayFlow.Application.DTOs.Employee;

public class EmployeeResponseDto
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Contact { get; set; }
    public string? Email { get; set; }
    public string? Aadhaar { get; set; }
    public DateTime DateOfJoining { get; set; }
    public decimal BasicPay { get; set; }
    public decimal ConveyanceAllowance { get; set; }
    public string? PhotoPath { get; set; }
    public bool IsActive { get; set; }
}
