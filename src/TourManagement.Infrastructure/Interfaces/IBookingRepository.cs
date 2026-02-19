using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Interfaces;

/// <summary>
/// Repository interface for Booking entity
/// </summary>
public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Booking>> GetByTourIdAsync(int tourId);
}
