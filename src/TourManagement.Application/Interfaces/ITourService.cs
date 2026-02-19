using TourManagement.Application.DTOs;

namespace TourManagement.Application.Interfaces;

/// <summary>
/// Service interface for Tour operations
/// </summary>
public interface ITourService
{
    Task<IEnumerable<TourDto>> GetAllToursAsync();
    Task<IEnumerable<TourDto>> GetActiveToursAsync();
    Task<TourDto?> GetTourByIdAsync(int id);
    Task<TourDto> CreateTourAsync(CreateTourDto createTourDto);
    Task<TourDto?> UpdateTourAsync(UpdateTourDto updateTourDto);
    Task<bool> DeleteTourAsync(int id);
    Task<IEnumerable<TourDto>> SearchToursAsync(string searchTerm);
}
