using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Repositories;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Application.Services;

public class TourService : ITourService
{
    private readonly ITourRepository _tourRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TourService> _logger;
    private readonly IValidator<TourCreateDto> _createValidator;
    private readonly IValidator<TourUpdateDto> _updateValidator;

    public TourService(
        ITourRepository tourRepository,
        IMapper mapper,
        ILogger<TourService> logger,
        IValidator<TourCreateDto> createValidator,
        IValidator<TourUpdateDto> updateValidator)
    {
        _tourRepository = tourRepository;
        _mapper = mapper;
        _logger = logger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IReadOnlyList<TourDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var tours = await _tourRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TourDto>>(tours);
    }

    public async Task<TourDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.GetByIdAsync(id, cancellationToken);
        return tour is null ? null : _mapper.Map<TourDto>(tour);
    }

    public async Task<TourDto> CreateAsync(TourCreateDto dto, CancellationToken cancellationToken)
    {
        await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);
        try
        {
            var entity = _mapper.Map<Tour>(dto);
            var created = await _tourRepository.AddAsync(entity, cancellationToken);
            _logger.LogInformation("Created tour {TourName}", created.Name);
            return _mapper.Map<TourDto>(created);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to create tour");
            throw;
        }
    }

    public async Task UpdateAsync(int id, TourUpdateDto dto, CancellationToken cancellationToken)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);
        var existing = await _tourRepository.GetByIdAsync(id, cancellationToken) ?? throw new InvalidOperationException("Tour not found.");
        _mapper.Map(dto, existing);
        existing.ModifiedDate = DateTime.UtcNow;
        await _tourRepository.UpdateAsync(existing, cancellationToken);
        _logger.LogInformation("Updated tour {TourId}", id);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _tourRepository.DeleteAsync(id, cancellationToken);
        _logger.LogInformation("Deleted tour {TourId}", id);
    }

    public async Task<IReadOnlyList<TourDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken)
    {
        var tours = await _tourRepository.SearchAsync(searchTerm, cancellationToken);
        return _mapper.Map<IReadOnlyList<TourDto>>(tours);
    }
}
