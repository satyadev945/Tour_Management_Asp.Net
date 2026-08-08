using TourManagement.Application.DTOs;

namespace TourManagement.Application.Contracts;

public interface IBookingService
{
    Task<IReadOnlyList<BookingDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<BookingDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<BookingDto> CreateAsync(BookingCreateDto dto, CancellationToken cancellationToken);
    Task UpdateAsync(int id, BookingUpdateDto dto, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<BookingDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken);
}
