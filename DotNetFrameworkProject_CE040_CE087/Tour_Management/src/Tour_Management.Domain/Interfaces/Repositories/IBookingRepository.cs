using Tour_Management.Domain.Entities;

namespace Tour_Management.Domain.Interfaces.Repositories;

public interface IBookingRepository
{
    Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Booking>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Booking> AddAsync(Booking booking, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
