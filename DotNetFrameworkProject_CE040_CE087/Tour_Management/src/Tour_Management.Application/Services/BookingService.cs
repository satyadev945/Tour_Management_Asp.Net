using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Repositories;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Application.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITourRepository _tourRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BookingService> _logger;
    private readonly IValidator<BookingCreateDto> _validator;

    public BookingService(
        IBookingRepository bookingRepository,
        ITourRepository tourRepository,
        IMapper mapper,
        ILogger<BookingService> logger,
        IValidator<BookingCreateDto> validator)
    {
        _bookingRepository = bookingRepository;
        _tourRepository = tourRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<IReadOnlyList<BookingDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<BookingDto>>(bookings);
    }

    public async Task<IReadOnlyList<BookingDto>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetByEmailAsync(email, cancellationToken);
        return _mapper.Map<IReadOnlyList<BookingDto>>(bookings);
    }

    public async Task<BookingDto> CreateAsync(BookingCreateDto dto, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(dto, cancellationToken);
        var tour = await _tourRepository.GetByIdAsync(dto.TourId, cancellationToken) ?? throw new InvalidOperationException("Tour not found.");
        var booking = _mapper.Map<Booking>(dto);
        booking.Tour = tour;
        var created = await _bookingRepository.AddAsync(booking, cancellationToken);
        _logger.LogInformation("Created booking for {Email}", created.Email);
        return _mapper.Map<BookingDto>(created);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _bookingRepository.DeleteAsync(id, cancellationToken);
        _logger.LogInformation("Deleted booking {BookingId}", id);
    }
}
