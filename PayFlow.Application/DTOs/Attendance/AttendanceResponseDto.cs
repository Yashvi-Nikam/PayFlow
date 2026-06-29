namespace PayFlow.Application.DTOs.Attendance;

public class AttendanceResponseDto
{
    public int AttendanceId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string? PhotoPath { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int WorkingDays { get; set; }
    public int DaysPresent { get; set; }
    public int PaidLeaves { get; set; }
    public int EffectiveWorkingDays { get; set; }
    public int UnpaidLeaves { get; set; }
    public int Leaves { get; set; }
    public decimal DeductionPreview { get; set; }
}
