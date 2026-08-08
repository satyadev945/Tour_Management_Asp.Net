using TourManagement.Domain.Entities;

namespace TourManagement.Domain.Interfaces.Repositories;

public interface IBookingRepository
{
    Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken);
    Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Booking> AddAsync(Booking entity, CancellationToken cancellationToken);
    Task UpdateAsync(Booking entity, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Booking>> SearchAsync(string searchTerm, CancellationToken cancellationToken);
}
