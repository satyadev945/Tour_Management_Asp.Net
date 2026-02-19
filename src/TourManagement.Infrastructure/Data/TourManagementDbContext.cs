using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data;

/// <summary>
/// Database context for Tour Management application
/// </summary>
public class TourManagementDbContext : DbContext
{
    public TourManagementDbContext(DbContextOptions<TourManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tour> Tours { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Tour entity
        modelBuilder.Entity<Tour>(entity =>
        {
            entity.ToTable("Tour");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TourName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Place).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Days).IsRequired();
            entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Locations).HasMaxLength(500);
            entity.Property(e => e.TourInfo).HasMaxLength(2000);
            entity.Property(e => e.PicturePath).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
        });

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Userinfo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure Booking entity
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Booking");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BookingDate).IsRequired();
            entity.Property(e => e.NumberOfPeople).IsRequired();
            entity.Property(e => e.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SpecialRequests).HasMaxLength(1000);
            entity.Property(e => e.CreatedDate).IsRequired();

            // Configure relationships
            entity.HasOne(e => e.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Tour)
                .WithMany(t => t.Bookings)
                .HasForeignKey(e => e.TourId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
