using Appointment.Domain;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Persistence.Context;

public class DataContext : DataContextBase, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<Service> Services { get; set; }
}