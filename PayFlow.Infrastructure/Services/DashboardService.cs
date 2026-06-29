using PayFlow.Application.DTOs.Dashboard;
using PayFlow.Application.Interfaces.Repositories;
using PayFlow.Application.Interfaces.Services;

namespace PayFlow.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IPayrollRepository _payrollRepo;

    public DashboardService(
        IEmployeeRepository employeeRepo,
        IPayrollRepository payrollRepo)
    {
        _employeeRepo = employeeRepo;
        _payrollRepo = payrollRepo;
    }

    public async Task<AdminDashboardDto> GetAdminDashboardAsync()
    {
        var now = DateTime.UtcNow;
        var currentMonth = now.Month;
        var currentYear = now.Year;

        // All active employees
        var allEmployees = await _employeeRepo.GetAllAsync();
        var activeEmployees = allEmployees.Where(e => e.IsActive).ToList();

        // Current month payrolls
        var currentPayrolls = await _payrollRepo
            .GetAllByMonthYearAsync(currentMonth, currentYear);
        var payrollList = currentPayrolls.ToList();

        // Stats
        var paidList = payrollList.Where(p => p.IsPaid).ToList();
        var pendingList = payrollList.Where(p => !p.IsPaid).ToList();

        // Pending payments list
        var pendingPayments = pendingList.Select(p => new PendingPaymentDto
        {
            EmployeeId = p.EmployeeId,
            EmployeeName = p.Employee?.Name ?? string.Empty,
            Designation = p.Employee?.Designation ?? string.Empty,
            PhotoPath = p.Employee?.PhotoPath,
            NetPay = p.NetPay,
            Month = p.Month,
            Year = p.Year
        });

        // Recent payrolls — last 3 months
        var recentPayrolls = new List<RecentPayrollSummaryDto>();
        for (int i = 0; i < 3; i++)
        {
            var month = currentMonth - i;
            var year = currentYear;
            if (month <= 0) { month += 12; year--; }

            var monthPayrolls = await _payrollRepo.GetAllByMonthYearAsync(month, year);
            var monthList = monthPayrolls.ToList();
            if (!monthList.Any()) continue;

            var paid = monthList.Count(p => p.IsPaid);
            var pending = monthList.Count(p => !p.IsPaid);

            recentPayrolls.Add(new RecentPayrollSummaryDto
            {
                Month = month,
                Year = year,
                TotalEmployees = monthList.Count,
                TotalPayroll = monthList.Sum(p => p.NetPay),
                Paid = paid,
                Pending = pending,
                Status = pending == 0 ? "Completed" : "In Progress"
            });
        }

        return new AdminDashboardDto
        {
            TotalEmployees = activeEmployees.Count,
            PaidEmployees = paidList.Count,
            PendingPayments = pendingList.Count,
            TotalPayroll = payrollList.Sum(p => p.NetPay),
            CurrentMonth = currentMonth,
            CurrentYear = currentYear,
            PendingPaymentsList = pendingPayments,
            RecentPayrolls = recentPayrolls
        };
    }
}
