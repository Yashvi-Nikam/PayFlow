namespace PayFlow.Application.DTOs.Payroll;

public class PayrollGenerateDto
{
    public int EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal Deduction { get; set; }
    public decimal Incentive { get; set; }
}
