using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Interfaces;

namespace TourManagement.Application.Services;

/// <summary>
/// Service implementation for Tour operations
/// </summary>
public class TourService : ITourService
{
    private readonly ITourRepository _tourRepository;
    private readonly ILogger<TourService> _logger;

    public TourService(ITourRepository tourRepository, ILogger<TourService> logger)
    {
        _tourRepository = tourRepository ?? throw new ArgumentNullException(nameof(tourRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<TourDto>> GetAllToursAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all tours");
            var tours = await _tourRepository.GetAllAsync();
            return tours.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all tours");
            throw;
        }
    }

    public async Task<IEnumerable<TourDto>> GetActiveToursAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving active tours");
            var tours = await _tourRepository.GetActiveToursAsync();
            return tours.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active tours");
            throw;
        }
    }

    public async Task<TourDto?> GetTourByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving tour with ID: {TourId}", id);
            var tour = await _tourRepository.GetByIdAsync(id);
            return tour != null ? MapToDto(tour) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tour with ID: {TourId}", id);
            throw;
        }
    }

    public async Task<TourDto> CreateTourAsync(CreateTourDto createTourDto)
    {
        try
        {
            _logger.LogInformation("Creating new tour: {TourName}", createTourDto.TourName);
            
            var tour = new Tour
            {
                TourName = createTourDto.TourName,
                Place = createTourDto.Place,
                Days = createTourDto.Days,
                Price = createTourDto.Price,
                Locations = createTourDto.Locations,
                TourInfo = createTourDto.TourInfo,
                PicturePath = createTourDto.PicturePath,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            var createdTour = await _tourRepository.AddAsync(tour);
            _logger.LogInformation("Tour created successfully with ID: {TourId}", createdTour.Id);
            
            return MapToDto(createdTour);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour: {TourName}", createTourDto.TourName);
            throw;
        }
    }

    public async Task<TourDto?> UpdateTourAsync(UpdateTourDto updateTourDto)
    {
        try
        {
            _logger.LogInformation("Updating tour with ID: {TourId}", updateTourDto.Id);
            
            var existingTour = await _tourRepository.GetByIdAsync(updateTourDto.Id);
            if (existingTour == null)
            {
                _logger.LogWarning("Tour with ID: {TourId} not found", updateTourDto.Id);
                return null;
            }

            existingTour.TourName = updateTourDto.TourName;
            existingTour.Place = updateTourDto.Place;
            existingTour.Days = updateTourDto.Days;
            existingTour.Price = updateTourDto.Price;
            existingTour.Locations = updateTourDto.Locations;
            existingTour.TourInfo = updateTourDto.TourInfo;
            existingTour.PicturePath = updateTourDto.PicturePath;
            existingTour.IsActive = updateTourDto.IsActive;
            existingTour.ModifiedDate = DateTime.UtcNow;

            var updatedTour = await _tourRepository.UpdateAsync(existingTour);
            _logger.LogInformation("Tour updated successfully with ID: {TourId}", updatedTour.Id);
            
            return MapToDto(updatedTour);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour with ID: {TourId}", updateTourDto.Id);
            throw;
        }
    }

    public async Task<bool> DeleteTourAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting tour with ID: {TourId}", id);
            var result = await _tourRepository.DeleteAsync(id);
            
            if (result)
            {
                _logger.LogInformation("Tour deleted successfully with ID: {TourId}", id);
            }
            else
            {
                _logger.LogWarning("Tour with ID: {TourId} not found for deletion", id);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with ID: {TourId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TourDto>> SearchToursAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation("Searching tours with term: {SearchTerm}", searchTerm);
            var tours = await _tourRepository.SearchToursAsync(searchTerm);
            return tours.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching tours with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    private static TourDto MapToDto(Tour tour)
    {
        return new TourDto
        {
            Id = tour.Id,
            TourName = tour.TourName,
            Place = tour.Place,
            Days = tour.Days,
            Price = tour.Price,
            Locations = tour.Locations,
            TourInfo = tour.TourInfo,
            PicturePath = tour.PicturePath,
            IsActive = tour.IsActive
        };
    }
}
