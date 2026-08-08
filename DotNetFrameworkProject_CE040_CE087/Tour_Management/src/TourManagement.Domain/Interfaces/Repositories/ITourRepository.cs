using TourManagement.Domain.Entities;

namespace TourManagement.Domain.Interfaces.Repositories;

public interface ITourRepository
{
    Task<IReadOnlyList<Tour>> GetAllAsync(CancellationToken cancellationToken);
    Task<Tour?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Tour> AddAsync(Tour entity, CancellationToken cancellationToken);
    Task UpdateAsync(Tour entity, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Tour>> SearchAsync(string searchTerm, CancellationToken cancellationToken);
}
