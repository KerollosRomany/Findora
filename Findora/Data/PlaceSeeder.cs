using Findora.Enums;
using Findora.Models;
using Microsoft.EntityFrameworkCore;

namespace Findora.Data
{
    public static class PlaceSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Places.AnyAsync()) return;

            var cities = await context.Cities.ToDictionaryAsync(c => c.Name, c => c.Id);
            var amenities = await context.Amenities.ToDictionaryAsync(a => a.Name, a => a.Id);

            var places = new List<Place>
            {
                new Place
                {
                    Name = "Quiet Side",
                    Type = PlaceType.WorkSpace,
                    Address = "Bazaar Street, El Dahar, opposite El Sonny Restaurant, Hurghada",
                    Description = "Great place to work or relax! Clean and comfortable workspace, with a nice variety of hot and cold drinks.",
                    CityId = cities["Hurghada"],
                    Latitude = 27.251694m,
                    Longitude = 33.818441m,
                    PlaceAmenities = new List<PlaceAmenity>
                    {
                        new() { AmenityId = amenities["Wi-Fi"] },
                        new() { AmenityId = amenities["Quiet Area"] },
                        new() { AmenityId = amenities["Drinks"] },
                        new() { AmenityId = amenities["Meeting Room"] }
                    },
                    WorkingHours = new List<PlaceWorkingHour>
                    {
                        new() { Day = DayOfWeek.Saturday,  OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(23, 0) },
                        new() { Day = DayOfWeek.Sunday,    OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(23, 0) },
                        new() { Day = DayOfWeek.Monday,    OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(23, 0) },
                        new() { Day = DayOfWeek.Tuesday,   OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(23, 0) },
                        new() { Day = DayOfWeek.Wednesday, OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(23, 0) },
                        new() { Day = DayOfWeek.Thursday,  OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(23, 0) },
                        new() { Day = DayOfWeek.Friday,    OpenTime = new TimeOnly(14, 0), CloseTime = new TimeOnly(23, 0) }
                    },
                    PlaceImages = new List<PlaceImage>
                    {
                        new() { ImgURL = "/images/seed/quietSide2.jpg", IsMain = true },
                        new() { ImgURL = "/images/seed/quietSide3.jpg", IsMain = false },
                        new() { ImgURL = "/images/seed/quietSide4.jpg", IsMain = false }
                    }
                },

                new Place
                {
                    Name = "Bibliotheca Alexandrina",
                    Type = PlaceType.Library,
                    Address = "El Shatby, Alexandria",
                    Description = "A landmark library and cultural centre on Alexandria's waterfront, with large quiet reading halls, Wi-Fi and printing services. An inspiring place for long study sessions.",
                    CityId = cities["Alexandria"],
                    Latitude = 31.208903m,
                    Longitude = 29.909159m,
                    PlaceAmenities = new List<PlaceAmenity>
                    {
                        new() { AmenityId = amenities["Printing & Copying"] },
                        new() { AmenityId = amenities["Wi-Fi"] },
                        new() { AmenityId = amenities["Quiet Area"] }
                    },
                    WorkingHours = new List<PlaceWorkingHour>
                    {
                        new() { Day = DayOfWeek.Saturday,  OpenTime = new TimeOnly(10, 0), CloseTime = new TimeOnly(14, 0) },
                        new() { Day = DayOfWeek.Sunday,    OpenTime = new TimeOnly(10, 0), CloseTime = new TimeOnly(19, 0) },
                        new() { Day = DayOfWeek.Monday,    OpenTime = new TimeOnly(10, 0), CloseTime = new TimeOnly(19, 0) },
                        new() { Day = DayOfWeek.Tuesday,   OpenTime = new TimeOnly(10, 0), CloseTime = new TimeOnly(19, 0) },
                        new() { Day = DayOfWeek.Wednesday, OpenTime = new TimeOnly(10, 0), CloseTime = new TimeOnly(19, 0) },
                        new() { Day = DayOfWeek.Thursday,  OpenTime = new TimeOnly(10, 0), CloseTime = new TimeOnly(19, 0) },
                        new() { Day = DayOfWeek.Friday,    IsClosed = true }
                    },
                    PlaceImages = new List<PlaceImage>
                    {
                        new() { ImgURL = "/images/seed/alex1.jpg", IsMain = true },
                        new() { ImgURL = "/images/seed/alex2.jpg", IsMain = false },
                        new() { ImgURL = "/images/seed/alex3.jpg", IsMain = false}

                    }
                },

                new Place
                {
                    Name = "Maxime's El Saltana Café",
                    Type = PlaceType.Cafe,
                    Address = "Luxor City, Luxor",   
                    Description = "A cosy, quiet café in Luxor with hot and cold drinks, desserts, rooftop seating and Wi-Fi. Open from morning until 2 AM, so it works for daytime study and late-night work.",
                    CityId = cities["Luxor"],
                    Latitude = 25.684673m,
                    Longitude = 32.630151m,
                    PlaceAmenities = new List<PlaceAmenity>
                    {
                        new() { AmenityId = amenities["Air Conditioning"] },
                        new() { AmenityId = amenities["Drinks"] },
                        new() { AmenityId = amenities["Wi-Fi"] }
                    },
                    WorkingHours = EveryDay(new TimeOnly(8, 0), new TimeOnly(2, 0)),
                    PlaceImages = new List<PlaceImage>
                    {
                        new() { ImgURL = "/images/seed/l1.jpg", IsMain = true },
                        new() { ImgURL = "/images/seed/l2.jpg", IsMain = false },
                        new() { ImgURL = "/images/seed/l3.jpg", IsMain = false },
                        new() { ImgURL = "/images/seed/l4.jpg", IsMain = false }
                    }
                },

                new Place
                {
                    Name = "Al Kotob Khan",
                    Type = PlaceType.Library,
                    Address = "13 Road 206, Degla Square, Maadi, Cairo",
                    Description = "A bookshop café in Maadi's Degla Square, a calm spot to read and work among the books with a relaxed atmosphere and good drinks.",
                    CityId = cities["Maadi"],
                    Latitude = 29.955585m,
                    Longitude = 31.273262m,
                    PlaceAmenities = new List<PlaceAmenity>
                    {
                        new() { AmenityId = amenities["Drinks"] },
                        new() { AmenityId = amenities["Quiet Area"] }
                    },
                    WorkingHours = EveryDay(new TimeOnly(9, 0), new TimeOnly(23, 0)),
                    PlaceImages = new List<PlaceImage>
                    {
                        new() { ImgURL = "/images/seed/khan1.jpg", IsMain = true },
                        new() { ImgURL = "/images/seed/khan2.jpg", IsMain = false },
                        new() { ImgURL = "/images/seed/khan3.jpg", IsMain = false }

                    }
                },

                new Place
                {
                    Name = "Innova Workspace",
                    Type = PlaceType.WorkSpace,
                    Address = "Mohamed Ali Makarem St, next to Sultana Restaurant, Al Walideyah Al Qebleyah, Assiut",
                    Description = "A modern coworking space in Assiut with comfortable desks and a focused atmosphere, suited to students and freelancers who need a quiet place to work.",
                    CityId = cities["Assiut"],
                    Latitude = 27.185673m,
                    Longitude = 31.177971m,
                    PlaceAmenities = new List<PlaceAmenity>
                    {
                        new() { AmenityId = amenities["Wi-Fi"] },
                        new() { AmenityId = amenities["Quiet Area"] }
                    },
                    WorkingHours = new List<PlaceWorkingHour>
                    {
                        new() { Day = DayOfWeek.Saturday,  OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(2, 0) },
                        new() { Day = DayOfWeek.Sunday,    OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(2, 0) },
                        new() { Day = DayOfWeek.Monday,    OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(2, 0) },
                        new() { Day = DayOfWeek.Tuesday,   OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(2, 0) },
                        new() { Day = DayOfWeek.Wednesday, OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(2, 0) },
                        new() { Day = DayOfWeek.Thursday,  OpenTime = new TimeOnly(9, 0),  CloseTime = new TimeOnly(2, 0) },
                        new() { Day = DayOfWeek.Friday,    OpenTime = new TimeOnly(14, 0), CloseTime = new TimeOnly(2, 0) }
                    },
                    PlaceImages = new List<PlaceImage>
                    {
                        new() { ImgURL = "/images/seed/innova1.jpg", IsMain = true },
                        new() { ImgURL = "/images/seed/innova2.jpg", IsMain = false },
                        new() { ImgURL = "/images/seed/innova3.jpg", IsMain = false }

                    }
                }
            };

            context.Places.AddRange(places);
            await context.SaveChangesAsync();
        }

        private static List<PlaceWorkingHour> EveryDay(TimeOnly open, TimeOnly close) =>
            Enum.GetValues<DayOfWeek>()
                .Select(d => new PlaceWorkingHour { Day = d, OpenTime = open, CloseTime = close })
                .ToList();
    }
}