namespace AppointmentService.Application.TimeSlots;

public class TimeSlotDto
{
    public Guid Id { get; set; }
    public string MasterId { get; set; } = string.Empty;
    public DayOfWeek DayOfWeek { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}