using Findora.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Findora.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Favourite> Favourites { get; set; } 
    public DbSet<Governorate> Governorates { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<PlaceAmenity> PlaceAmenities { get; set; }
    public DbSet<PlaceImage> PlaceImages { get; set; }

    public DbSet<PlaceSuggestion> PlaceSuggestions { get; set; }
    public DbSet<PlaceWorkingHour> WorkingHours { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}