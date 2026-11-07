using Findora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Findora.Configurations;

public class FavouriteConfiguration : IEntityTypeConfiguration<Favourites>
{
    public void Configure(EntityTypeBuilder<Favourites> builder)
    {
        builder.HasKey(x => new { x.UserId, x.PlaceId });

        builder.HasOne(x => x.User)
            .WithMany(x => x.Favorites)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Place)
            .WithMany(x => x.Favourites)
            .HasForeignKey(x => x.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
