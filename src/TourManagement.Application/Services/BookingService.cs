using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Interfaces;

namespace TourManagement.Application.Services;

/// <summary>
/// Service implementation for Booking operations
/// </summary>
public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITourRepository _tourRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<BookingService> _logger;

    public BookingService(
        IBookingRepository bookingRepository,
        ITourRepository tourRepository,
        IUserRepository userRepository,
        ILogger<BookingService> logger)
    {
        _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        _tourRepository = tourRepository ?? throw new ArgumentNullException(nameof(tourRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all bookings");
            var bookings = await _bookingRepository.GetAllAsync();
            return await MapToDtoListAsync(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all bookings");
            throw;
        }
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(int userId)
    {
        try
        {
            _logger.LogInformation("Retrieving bookings for user ID: {UserId}", userId);
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);
            return await MapToDtoListAsync(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bookings for user ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByTourIdAsync(int tourId)
    {
        try
        {
            _logger.LogInformation("Retrieving bookings for tour ID: {TourId}", tourId);
            var bookings = await _bookingRepository.GetByTourIdAsync(tourId);
            return await MapToDtoListAsync(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bookings for tour ID: {TourId}", tourId);
            throw;
        }
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving booking with ID: {BookingId}", id);
            var booking = await _bookingRepository.GetByIdAsync(id);
            return booking != null ? await MapToDtoAsync(booking) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving booking with ID: {BookingId}", id);
            throw;
        }
    }

    public async Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto)
    {
        try
        {
            _logger.LogInformation("Creating new booking for user ID: {UserId}, tour ID: {TourId}", 
                createBookingDto.UserId, createBookingDto.TourId);
            
            // Validate user exists
            var user = await _userRepository.GetByIdAsync(createBookingDto.UserId);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {createBookingDto.UserId} not found");
            }

            // Validate tour exists
            var tour = await _tourRepository.GetByIdAsync(createBookingDto.TourId);
            if (tour == null)
            {
                throw new InvalidOperationException($"Tour with ID {createBookingDto.TourId} not found");
            }

            // Calculate total amount
            var totalAmount = tour.Price * createBookingDto.NumberOfPeople;

            var booking = new Booking
            {
                UserId = createBookingDto.UserId,
                TourId = createBookingDto.TourId,
                BookingDate = createBookingDto.BookingDate,
                NumberOfPeople = createBookingDto.NumberOfPeople,
                TotalAmount = totalAmount,
                Status = "Pending",
                SpecialRequests = createBookingDto.SpecialRequests,
                CreatedDate = DateTime.UtcNow
            };

            var createdBooking = await _bookingRepository.AddAsync(booking);
            _logger.LogInformation("Booking created successfully with ID: {BookingId}", createdBooking.Id);
            
            return await MapToDtoAsync(createdBooking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for user ID: {UserId}, tour ID: {TourId}", 
                createBookingDto.UserId, createBookingDto.TourId);
            throw;
        }
    }

    public async Task<BookingDto?> UpdateBookingStatusAsync(UpdateBookingStatusDto updateStatusDto)
    {
        try
        {
            _logger.LogInformation("Updating booking status for ID: {BookingId} to {Status}", 
                updateStatusDto.Id, updateStatusDto.Status);
            
            var existingBooking = await _bookingRepository.GetByIdAsync(updateStatusDto.Id);
            if (existingBooking == null)
            {
                _logger.LogWarning("Booking with ID: {BookingId} not found", updateStatusDto.Id);
                return null;
            }

            existingBooking.Status = updateStatusDto.Status;
            existingBooking.ModifiedDate = DateTime.UtcNow;

            if (updateStatusDto.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                existingBooking.CancellationDate = DateTime.UtcNow;
            }

            var updatedBooking = await _bookingRepository.UpdateAsync(existingBooking);
            _logger.LogInformation("Booking status updated successfully for ID: {BookingId}", updatedBooking.Id);
            
            return await MapToDtoAsync(updatedBooking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking status for ID: {BookingId}", updateStatusDto.Id);
            throw;
        }
    }

    public async Task<bool> CancelBookingAsync(int id)
    {
        try
        {
            _logger.LogInformation("Cancelling booking with ID: {BookingId}", id);
            
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                _logger.LogWarning("Booking with ID: {BookingId} not found", id);
                return false;
            }

            booking.Status = "Cancelled";
            booking.CancellationDate = DateTime.UtcNow;
            booking.ModifiedDate = DateTime.UtcNow;

            await _bookingRepository.UpdateAsync(booking);
            _logger.LogInformation("Booking cancelled successfully with ID: {BookingId}", id);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking with ID: {BookingId}", id);
            throw;
        }
    }

    private async Task<BookingDto> MapToDtoAsync(Booking booking)
    {
        var user = await _userRepository.GetByIdAsync(booking.UserId);
        var tour = await _tourRepository.GetByIdAsync(booking.TourId);

        return new BookingDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            TourId = booking.TourId,
            UserName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown",
            TourName = tour?.TourName ?? "Unknown",
            BookingDate = booking.BookingDate,
            NumberOfPeople = booking.NumberOfPeople,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            SpecialRequests = booking.SpecialRequests
        };
    }

    private async Task<IEnumerable<BookingDto>> MapToDtoListAsync(IEnumerable<Booking> bookings)
    {
        var bookingDtos = new List<BookingDto>();
        foreach (var booking in bookings)
        {
            bookingDtos.Add(await MapToDtoAsync(booking));
        }
        return bookingDtos;
    }
}
