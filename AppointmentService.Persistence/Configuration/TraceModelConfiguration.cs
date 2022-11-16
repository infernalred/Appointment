using AppointmentService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentService.Persistence.Configuration;

public class TraceModelConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.MachineName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Level).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Message).IsRequired();
        builder.Property(x => x.Logger).HasMaxLength(250);
    }
}