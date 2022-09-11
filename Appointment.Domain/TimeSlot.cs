namespace Appointment.Domain;

public class TimeSlot
{
    public int Id { get; set; }
    public string MasterId { get; set; } = string.Empty;
    public Master? Master { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}