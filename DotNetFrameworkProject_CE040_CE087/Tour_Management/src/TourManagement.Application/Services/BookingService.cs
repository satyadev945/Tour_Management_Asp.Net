using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Application.Contracts;

namespace TourManagement.Application.Services;

public sealed class BookingService : IBookingService
{
    private readonly IBookingRepository _repository;
    private readonly ITourRepository _tourRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BookingService> _logger;
    private readonly IValidator<BookingCreateDto> _createValidator;
    private readonly IValidator<BookingUpdateDto> _updateValidator;

    public BookingService(
        IBookingRepository repository,
        ITourRepository tourRepository,
        IMapper mapper,
        ILogger<BookingService> logger,
        IValidator<BookingCreateDto> createValidator,
        IValidator<BookingUpdateDto> updateValidator)
    {
        _repository = repository;
        _tourRepository = tourRepository;
        _mapper = mapper;
        _logger = logger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IReadOnlyList<BookingDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<BookingDto>>(entities);
    }

    public async Task<BookingDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : _mapper.Map<BookingDto>(entity);
    }

    public async Task<BookingDto> CreateAsync(BookingCreateDto dto, CancellationToken cancellationToken)
    {
        await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);
        var tour = await _tourRepository.GetByIdAsync(dto.TourId, cancellationToken) ?? throw new EntityNotFoundException($"Tour {dto.TourId} was not found.");
        var entity = _mapper.Map<Booking>(dto);
        entity.TourId = tour.Id;
        var created = await _repository.AddAsync(entity, cancellationToken);
        _logger.LogInformation("Created booking for {Email}", created.Email);
        return _mapper.Map<BookingDto>(created);
    }

    public async Task UpdateAsync(int id, BookingUpdateDto dto, CancellationToken cancellationToken)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);
        var existing = await _repository.GetByIdAsync(id, cancellationToken) ?? throw new EntityNotFoundException($"Booking {id} was not found.");
        existing.TourId = dto.TourId;
        existing.CustomerName = dto.CustomerName;
        existing.Email = dto.Email;
        existing.City = dto.City;
        existing.PhoneNumber = dto.PhoneNumber;
        existing.ModifiedDate = DateTime.UtcNow;
        await _repository.UpdateAsync(existing, cancellationToken);
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken) => _repository.DeleteAsync(id, cancellationToken);

    public async Task<IReadOnlyList<BookingDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken)
    {
        var entities = await _repository.SearchAsync(searchTerm, cancellationToken);
        return _mapper.Map<IReadOnlyList<BookingDto>>(entities);
    }
}
