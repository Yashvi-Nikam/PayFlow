namespace PayFlow.Domain.Entities;

public class Employee
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
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
}