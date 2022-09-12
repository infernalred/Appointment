using Appointment.Domain;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Persistence.Context;

public class DataContext : DataContextBase, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<Master> Masters { get; set; } = null!;
    public DbSet<TimeSlot> TimeSlots { get; set; } = null!;
    public DbSet<Domain.Appointment> Appointments { get; set; } = null!;
}