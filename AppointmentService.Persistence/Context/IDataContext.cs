using AppointmentService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.Persistence.Context;

public interface IDataContext
{
    DbSet<AppUser> Users { get; set; }
    DbSet<Service> Services { get; set; }
    DbSet<Master> Masters { get; set; }
    DbSet<TimeSlot> TimeSlots { get; set; }
    DbSet<Appointment> Appointments { get; set; }
}