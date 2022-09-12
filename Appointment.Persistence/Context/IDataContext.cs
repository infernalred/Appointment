using Appointment.Domain;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Persistence.Context;

public interface IDataContext
{
    DbSet<AppUser> Users { get; set; }
    DbSet<Service> Services { get; set; }
    DbSet<Master> Masters { get; set; }
    DbSet<TimeSlot> TimeSlots { get; set; }
    DbSet<Domain.Appointment> Appointments { get; set; }
}