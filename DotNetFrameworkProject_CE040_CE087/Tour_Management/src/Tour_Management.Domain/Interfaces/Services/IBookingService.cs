namespace Tour_Management.Domain.Interfaces.Services;

public interface IBookingService
{
    Task<IReadOnlyList<Tour_Management.Application.DTOs.BookingDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Tour_Management.Application.DTOs.BookingDto>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Tour_Management.Application.DTOs.BookingDto> CreateAsync(Tour_Management.Application.DTOs.BookingCreateDto dto, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
