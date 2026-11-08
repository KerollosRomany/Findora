using Findora.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Findora.Data;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Student = "Student";
}

public static class DbSeeder
{
    private const string AdminEmail = "KerollosRomany@findora.com";
    private const string AdminFullName = "KerollosRomany";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<AppDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = services.GetRequiredService<IConfiguration>();

        await SeedRolesAsync(roleManager);
        await SeedAdminAsync(userManager, configuration);
        await SeedLocationsAsync(context);
        await SeedAmenitiesAsync(context);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { AppRoles.Admin, AppRoles.Student };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        var admin = await userManager.FindByEmailAsync(AdminEmail);

        if (admin == null)
        {
            var password = configuration["Admin:Password"];

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Admin:Password is missing");
            }

            admin = new ApplicationUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                EmailConfirmed = true,
                FullName = AdminFullName
            };

            var result = await userManager.CreateAsync(admin, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create the admin user: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(admin, AppRoles.Admin))
        {
            await userManager.AddToRoleAsync(admin, AppRoles.Admin);
        }
    }

    private static async Task SeedLocationsAsync(AppDbContext context)
    {
        if (await context.Governorates.AnyAsync())
            return;

        var governorates = new List<Governorate>
        {
            new()
            {
                Name = "Red Sea",
                Cities = { new City { Name = "Hurghada" }, new City { Name = "El Gouna" } }
            },
            new()
            {
                Name = "Luxor",
                Cities = { new City { Name = "Luxor" } }
            },
            new()
            {
                Name = "Alexandria",
                Cities = { new City { Name = "Alexandria" } }
            },
            new()
            {
                Name = "Cairo",
                Cities = { new City { Name = "Maadi" }, new City { Name = "Nasr City" } }
            },
            new()
            {
                Name = "Qena",
                Cities = { new City { Name = "Qena" } }
            },
            new()
            {
                Name = "Assiut",
                Cities = { new City { Name = "Assiut" } }
            }
        };

        context.Governorates.AddRange(governorates);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAmenitiesAsync(AppDbContext context)
    {
        if (await context.Amenities.AnyAsync())
            return;

        var amenities = new[]
        {
            "Wi-Fi",
            "Printing & Copying",
            "Meeting Room",
            "Drinks",
            "Air Conditioning",
            "Quiet Area"
        };

        context.Amenities.AddRange(amenities.Select(name => new Amenity { Name = name }));
        await context.SaveChangesAsync();
    }
}