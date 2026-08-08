namespace Tour_Management.Domain.Interfaces.Services;

public interface ITourService
{
    Task<IReadOnlyList<Tour_Management.Application.DTOs.TourDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<Tour_Management.Application.DTOs.TourDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Tour_Management.Application.DTOs.TourDto> CreateAsync(Tour_Management.Application.DTOs.TourCreateDto dto, CancellationToken cancellationToken);
    Task UpdateAsync(int id, Tour_Management.Application.DTOs.TourUpdateDto dto, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Tour_Management.Application.DTOs.TourDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken);
}
