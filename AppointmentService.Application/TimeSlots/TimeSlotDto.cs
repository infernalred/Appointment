namespace AppointmentService.Application.TimeSlots;

public class TimeSlotDto
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    private DateTime _start;

    public DateTime Start
    {
        get => new(_start.Year, _start.Month, _start.Day, _start.Hour, _start.Minute, 0, _start.Kind);
        set => _start = value;
    }

    private DateTime _end;
    public DateTime End
    {
        get => new(_end.Year, _end.Month, _end.Day, _end.Hour, _end.Minute, 0, _end.Kind);
        set => _end = value;
    }
}