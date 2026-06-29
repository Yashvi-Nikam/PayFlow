namespace PayFlow.Domain.Entities;

public class Attendance
{
    public int AttendanceId { get; set; }
    public int EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int WorkingDays { get; set; }
    public int DaysPresent { get; set; }
    public int PaidLeaves { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Derived
    public int EffectiveWorkingDays => WorkingDays - PaidLeaves;
    public int UnpaidLeaves => Math.Max(0, EffectiveWorkingDays - DaysPresent);
    public int Leaves => UnpaidLeaves; // kept for compatibility

    public Employee? Employee { get; set; }
}

