using System.Collections.Generic;

namespace AppointmentService.Domain;

public class Master
{
    public string Id { get; set; } = string.Empty;
    public AppUser User { get; set; } = null!;
    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}