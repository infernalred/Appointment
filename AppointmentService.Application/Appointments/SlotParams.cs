namespace AppointmentService.Application.Appointments;

public class SlotParams
{
    public DateTime Start { get; set; } = DateTime.UtcNow;
    public DateTime End { get; set; } = DateTime.UtcNow.AddDays(6);
}