using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Application.Contracts;

namespace TourManagement.Application.Services;

public sealed class TourService : ITourService
{
    private readonly ITourRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TourService> _logger;
    private readonly IValidator<TourCreateDto> _createValidator;
    private readonly IValidator<TourUpdateDto> _updateValidator;

    public TourService(
        ITourRepository repository,
        IMapper mapper,
        ILogger<TourService> logger,
        IValidator<TourCreateDto> createValidator,
        IValidator<TourUpdateDto> updateValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IReadOnlyList<TourDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TourDto>>(entities);
    }

    public async Task<TourDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : _mapper.Map<TourDto>(entity);
    }

    public async Task<TourDto> CreateAsync(TourCreateDto dto, CancellationToken cancellationToken)
    {
        await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);
        try
        {
            var entity = _mapper.Map<Tour>(dto);
            var created = await _repository.AddAsync(entity, cancellationToken);
            _logger.LogInformation("Created tour {TourName}", created.Name);
            return _mapper.Map<TourDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create tour");
            throw;
        }
    }

    public async Task UpdateAsync(int id, TourUpdateDto dto, CancellationToken cancellationToken)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);
        var existing = await _repository.GetByIdAsync(id, cancellationToken) ?? throw new EntityNotFoundException($"Tour {id} was not found.");
        existing.Name = dto.Name;
        existing.Place = dto.Place;
        existing.Days = dto.Days;
        existing.Price = dto.Price;
        existing.Locations = dto.Locations;
        existing.Description = dto.Description;
        existing.ImagePath = dto.ImagePath;
        existing.ModifiedDate = DateTime.UtcNow;
        await _repository.UpdateAsync(existing, cancellationToken);
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken) => _repository.DeleteAsync(id, cancellationToken);

    public async Task<IReadOnlyList<TourDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken)
    {
        var entities = await _repository.SearchAsync(searchTerm, cancellationToken);
        return _mapper.Map<IReadOnlyList<TourDto>>(entities);
    }
}
