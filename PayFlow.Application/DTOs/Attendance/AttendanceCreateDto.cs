namespace PayFlow.Application.DTOs.Attendance;

public class AttendanceCreateDto
{
    public int EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int WorkingDays { get; set; }
    public int DaysPresent { get; set; }
    public int PaidLeaves { get; set; }
}
