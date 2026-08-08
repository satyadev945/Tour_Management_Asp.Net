using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(TourManagementDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!await context.Tours.AnyAsync())
        {
            var tours = new[]
            {
                new Tour { Name = "Goa Escape", Place = "Goa", Days = 5, Price = 499.99m, Locations = "Beach, Market, Cruise", Description = "Relaxing coastal getaway.", ImagePath = "/images/goa.jpg" },
                new Tour { Name = "Kerala Backwaters", Place = "Kerala", Days = 7, Price = 699.99m, Locations = "Alleppey, Munnar, Kochi", Description = "Scenic backwater and hill station tour.", ImagePath = "/images/kerala.jpg" },
                new Tour { Name = "Kashmir Adventure", Place = "Kashmir", Days = 6, Price = 799.99m, Locations = "Srinagar, Gulmarg, Pahalgam", Description = "Mountain landscapes and adventure activities.", ImagePath = "/images/kashmir.jpg" }
            };

            await context.Tours.AddRangeAsync(tours);
        }

        if (!await context.Users.AnyAsync())
        {
            await context.Users.AddRangeAsync(
                new ApplicationUser
                {
                    Email = "admin@tourmanagement.local",
                    FirstName = "Admin",
                    LastName = "User",
                    Gender = "Other",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Street = "1 Admin Street",
                    City = "Admin City",
                    State = "Admin State",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    CreatedBy = "seed"
                },
                new ApplicationUser
                {
                    Email = "traveler@example.com",
                    FirstName = "Travel",
                    LastName = "Guest",
                    Gender = "Female",
                    DateOfBirth = new DateTime(1995, 5, 10),
                    Street = "22 Main Road",
                    City = "Mumbai",
                    State = "Maharashtra",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                    CreatedBy = "seed"
                });
        }

        await context.SaveChangesAsync();
    }
}
