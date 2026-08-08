using Tour_Management.Domain.Entities;

namespace Tour_Management.Domain.Interfaces.Repositories;

public interface IUserProfileRepository
{
    Task<ApplicationUserProfile?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken);
    Task<ApplicationUserProfile> AddAsync(ApplicationUserProfile profile, CancellationToken cancellationToken);
    Task UpdateAsync(ApplicationUserProfile profile, CancellationToken cancellationToken);
}
