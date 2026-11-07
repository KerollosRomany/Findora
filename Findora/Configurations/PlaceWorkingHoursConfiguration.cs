using Findora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Findora.Configurations;

public class PlaceWorkingHoursConfiguration : IEntityTypeConfiguration<PlaceWorkingHours>
{
    public void Configure(EntityTypeBuilder<PlaceWorkingHours> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(w => new { w.PlaceId, w.Day, w.OpenTime }).IsUnique();

        builder.Property(x => x.IsClosed)
            .HasDefaultValue(false);

        builder.HasOne(x => x.Place)
            .WithMany(x => x.WorkingHours)
            .HasForeignKey(x => x.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
