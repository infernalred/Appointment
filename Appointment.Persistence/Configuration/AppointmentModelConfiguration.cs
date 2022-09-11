using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointment.Persistence.Configuration;

public class AppointmentModelConfiguration : IEntityTypeConfiguration<Domain.Appointment>
{
    public void Configure(EntityTypeBuilder<Domain.Appointment> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(15);
    }
}