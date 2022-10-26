using AppointmentService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentService.Persistence.Configuration;

public class AppointmentModelConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(25);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(15);
    }
}