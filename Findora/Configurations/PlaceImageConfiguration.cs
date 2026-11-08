using Findora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Findora.Configurations;

public class PlaceImageConfiguration : IEntityTypeConfiguration<PlaceImage>
{
    public void Configure(EntityTypeBuilder<PlaceImage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ImgURL)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.IsMain)
            .HasDefaultValue(false);

        // SQL Server filtered unique index: each Place can have at most one main image.
        builder.HasIndex(x => x.PlaceId)
            .IsUnique()
            .HasFilter("[IsMain] = 1");

        builder.HasOne(x => x.Place)
            .WithMany(x => x.PlaceImages)
            .HasForeignKey(x => x.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
