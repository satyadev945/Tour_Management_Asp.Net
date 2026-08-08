using Tour_Management.Domain.Entities;

namespace Tour_Management.Domain.Interfaces.Repositories;

public interface ITourRepository
{
    Task<IReadOnlyList<Tour>> GetAllAsync(CancellationToken cancellationToken);
    Task<Tour?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Tour> AddAsync(Tour tour, CancellationToken cancellationToken);
    Task UpdateAsync(Tour tour, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Tour>> SearchAsync(string searchTerm, CancellationToken cancellationToken);
}
