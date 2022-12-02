namespace AppointmentService.Application.Masters;

public class SlotParams
{
    public DateTime Start { get; init; } = DateTime.UtcNow;
    public int QuantityDays { get; init; } = 6;
}