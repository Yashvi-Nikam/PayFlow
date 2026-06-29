namespace PayFlow.Domain.Entities;

public class Payroll
{
    public int PayrollId { get; set; }
    public int EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal BasicPay { get; set; }
    public decimal Allowance { get; set; }
    public decimal Deduction { get; set; }
    public decimal NetPay { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateOnly? PaidDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public decimal Incentive { get; set; } = 0;
    public Employee? Employee { get; set; }
}
