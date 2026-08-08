using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tour_Management.Domain.Entities;

namespace Tour_Management.Infrastructure.Data.Configurations;

public class ApplicationUserProfileConfiguration : IEntityTypeConfiguration<ApplicationUserProfile>
{
    public void Configure(EntityTypeBuilder<ApplicationUserProfile> builder)
    {
        builder.ToTable("UserProfiles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.IdentityUserId).IsRequired().HasMaxLength(450);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Gender).HasMaxLength(50);
        builder.Property(x => x.Street).HasMaxLength(200);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.State).HasMaxLength(100);
        builder.HasIndex(x => x.IdentityUserId).IsUnique();
    }
}
