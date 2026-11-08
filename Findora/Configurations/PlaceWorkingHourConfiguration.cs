using Findora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Findora.Configurations;

public class PlaceWorkingHourConfiguration : IEntityTypeConfiguration<PlaceWorkingHour>
{
    public void Configure(EntityTypeBuilder<PlaceWorkingHour> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(w => new { w.PlaceId, w.Day, w.OpenTime })
            .IsUnique()
            .HasFilter(null);

        builder.Property(x => x.IsClosed)
            .HasDefaultValue(false);

        builder.HasOne(x => x.Place)
            .WithMany(x => x.WorkingHours)
            .HasForeignKey(x => x.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
