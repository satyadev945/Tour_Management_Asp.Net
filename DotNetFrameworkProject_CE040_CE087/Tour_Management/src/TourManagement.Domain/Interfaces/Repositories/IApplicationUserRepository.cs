using TourManagement.Domain.Entities;

namespace TourManagement.Domain.Interfaces.Repositories;

public interface IApplicationUserRepository
{
    Task<IReadOnlyList<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApplicationUser?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<ApplicationUser> AddAsync(ApplicationUser entity, CancellationToken cancellationToken);
    Task UpdateAsync(ApplicationUser entity, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ApplicationUser>> SearchAsync(string searchTerm, CancellationToken cancellationToken);
}
