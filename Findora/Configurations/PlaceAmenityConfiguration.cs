using Findora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Findora.Configurations;

public class PlaceAmenityConfiguration : IEntityTypeConfiguration<PlaceAmenity>
{
    public void Configure(EntityTypeBuilder<PlaceAmenity> builder)
    {
        builder.HasKey(x => new { x.PlaceId, x.AmenityId });

        builder.HasOne(x => x.Place)
            .WithMany(x => x.PlaceAmenities)
            .HasForeignKey(x => x.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Amenity)
            .WithMany(x => x.PlaceAmenities)
            .HasForeignKey(x => x.AmenityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
