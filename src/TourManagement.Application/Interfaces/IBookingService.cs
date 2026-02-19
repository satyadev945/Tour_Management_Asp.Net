using TourManagement.Application.DTOs;

namespace TourManagement.Application.Interfaces;

/// <summary>
/// Service interface for Booking operations
/// </summary>
public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
    Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(int userId);
    Task<IEnumerable<BookingDto>> GetBookingsByTourIdAsync(int tourId);
    Task<BookingDto?> GetBookingByIdAsync(int id);
    Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto);
    Task<BookingDto?> UpdateBookingStatusAsync(UpdateBookingStatusDto updateStatusDto);
    Task<bool> CancelBookingAsync(int id);
}
