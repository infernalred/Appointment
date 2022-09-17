using AppointmentService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.Persistence.Context;

public class DataContext : DataContextBase, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<Master> Masters { get; set; } = null!;
    public DbSet<TimeSlot> TimeSlots { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
}