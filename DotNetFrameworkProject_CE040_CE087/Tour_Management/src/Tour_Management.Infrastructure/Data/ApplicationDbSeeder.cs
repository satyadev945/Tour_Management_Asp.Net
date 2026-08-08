using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data;

public class ApplicationDbSeeder
{
    private readonly TourManagementDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<ApplicationDbSeeder> _logger;

    public ApplicationDbSeeder(
        TourManagementDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<ApplicationDbSeeder> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        if (!await _roleManager.RoleExistsAsync("Administrator"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Administrator"));
        }

        var adminEmail = "admin@tourmanagement.local";
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Administrator");
            }
        }

        if (!await _context.Tours.AnyAsync())
        {
            _context.Tours.AddRange(
                new Tour
                {
                    Name = "Goa Explorer",
                    Place = "Goa",
                    Days = 5,
                    Price = 499.99m,
                    Locations = "North Goa, South Goa, Beaches",
                    Description = "A relaxing coastal tour package.",
                    ImagePath = "/Tour_pics/goa.jpg"
                },
                new Tour
                {
                    Name = "Kerala Escape",
                    Place = "Kerala",
                    Days = 7,
                    Price = 799.99m,
                    Locations = "Munnar, Alleppey, Kochi",
                    Description = "Backwaters and hill stations in one itinerary.",
                    ImagePath = "/Tour_pics/kerala.jpeg"
                });

            await _context.SaveChangesAsync();
            _logger.LogInformation("Seeded initial tours.");
        }
    }
}
