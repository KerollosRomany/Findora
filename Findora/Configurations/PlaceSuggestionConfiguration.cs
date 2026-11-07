using Findora.Enums;
using Findora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Findora.Configurations;

public class PlaceSuggestionConfiguration : IEntityTypeConfiguration<PlaceSuggestion>
{
    public void Configure(EntityTypeBuilder<PlaceSuggestion> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasDefaultValue(PlaceSuggestionStatus.Pending);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany(x => x.PlaceSuggestions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
