using Appointment.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointment.Persistence.Configuration;

public class ServiceModelConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(25);
        builder.Property(x => x.DurationMinutes).HasDefaultValue(30);
        builder.Property(x => x.IsEnabled).HasDefaultValue(true);
    }
}