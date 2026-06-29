namespace PayFlow.Application.DTOs.Payroll;

public class PayrollResponseDto
{
    public int PayrollId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal BasicPay { get; set; }
    public decimal Allowance { get; set; }
    public decimal Deduction { get; set; }
    public decimal NetPay { get; set; }
    public bool IsPaid { get; set; }
    public DateOnly? PaidDate { get; set; }
    public decimal Incentive { get; set; }
    public string? PhotoPath { get; set; }
}
