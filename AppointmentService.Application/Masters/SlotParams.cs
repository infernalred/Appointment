namespace AppointmentService.Application.Masters;

public class SlotParams
{
    public DateTime Start { get; set; } = DateTime.UtcNow;
    public DateTime End => Start.AddDays(6);
}