using Microsoft.EntityFrameworkCore;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Repositories;
using Tour_Management.Infrastructure.Data;

namespace Tour_Management.Infrastructure.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly TourManagementDbContext _context;

    public UserProfileRepository(TourManagementDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUserProfile?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken)
        => await _context.UserProfiles.AsNoTracking().FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId, cancellationToken);

    public async Task<ApplicationUserProfile> AddAsync(ApplicationUserProfile profile, CancellationToken cancellationToken)
    {
        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync(cancellationToken);
        return profile;
    }

    public async Task UpdateAsync(ApplicationUserProfile profile, CancellationToken cancellationToken)
    {
        _context.UserProfiles.Update(profile);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
