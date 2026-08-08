using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data;

public sealed class TourManagementDbContext : DbContext
{
    public TourManagementDbContext(DbContextOptions<TourManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TourManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
