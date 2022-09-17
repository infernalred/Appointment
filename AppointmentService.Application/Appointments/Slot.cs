namespace AppointmentService.Application.Appointments;

public class Slot
{
    public Guid Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}