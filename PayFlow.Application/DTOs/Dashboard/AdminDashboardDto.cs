namespace PayFlow.Application.DTOs.Dashboard;

public class AdminDashboardDto
{
    public int TotalEmployees { get; set; }
    public int PaidEmployees { get; set; }
    public int PendingPayments { get; set; }
    public decimal TotalPayroll { get; set; }
    public int CurrentMonth { get; set; }
    public int CurrentYear { get; set; }
    public IEnumerable<PendingPaymentDto> PendingPaymentsList { get; set; }
        = new List<PendingPaymentDto>();
    public IEnumerable<RecentPayrollSummaryDto> RecentPayrolls { get; set; }
        = new List<RecentPayrollSummaryDto>();
}

public class PendingPaymentDto
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string? PhotoPath { get; set; }
    public decimal NetPay { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

public class RecentPayrollSummaryDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public int TotalEmployees { get; set; }
    public decimal TotalPayroll { get; set; }
    public int Paid { get; set; }
    public int Pending { get; set; }
    public string Status { get; set; } = string.Empty;
}