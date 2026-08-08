using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Data.Configurations;

public sealed class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.ToTable("Tours");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Place).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Locations).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Price).HasColumnType("TEXT");
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ModifiedBy).HasMaxLength(100);
        builder.HasMany(x => x.Bookings)
            .WithOne(x => x.Tour)
            .HasForeignKey(x => x.TourId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
