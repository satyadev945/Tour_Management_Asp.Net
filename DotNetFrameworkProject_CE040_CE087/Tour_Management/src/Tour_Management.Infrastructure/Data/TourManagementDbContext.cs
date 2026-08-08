using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data;

public class TourManagementDbContext : IdentityDbContext
{
    public TourManagementDbContext(DbContextOptions<TourManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<ApplicationUserProfile> UserProfiles => Set<ApplicationUserProfile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(TourManagementDbContext).Assembly);
    }
}
