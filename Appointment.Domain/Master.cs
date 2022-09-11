namespace Appointment.Domain;

public class Master
{
    public string Id { get; set; } = string.Empty;
    public AppUser? User { get; set; }
    public int ServiceId { get; set; }
    public Service? Service { get; set; }
    public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
}