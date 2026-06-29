using PayFlow.Application.DTOs.Payroll;

namespace PayFlow.Application.Interfaces.Services;

public interface IPayrollService
{
    Task GenerateAsync(PayrollGenerateDto dto);
    Task UpdateDeductionAsync(int payrollId, PayrollUpdateDto dto);
    Task MarkAsPaidAsync(int payrollId);
    Task<PayrollResponseDto> GetByIdAsync(int payrollId);
    Task<IEnumerable<PayrollResponseDto>> GetAllByMonthYearAsync(int month, int year);
    Task<IEnumerable<PayrollResponseDto>> GetByEmployeeIdAsync(int employeeId);
    Task RecalculateAsync(int payrollId);
}