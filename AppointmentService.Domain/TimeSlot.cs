namespace AppointmentService.Domain;

public class TimeSlot
{
    public int Id { get; set; }
    public string MasterId { get; set; } = string.Empty;
    public Master? Master { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}