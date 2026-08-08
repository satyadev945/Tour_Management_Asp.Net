using TourManagement.Application.DTOs;

namespace TourManagement.Application.Contracts;

public interface ITourService
{
    Task<IReadOnlyList<TourDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<TourDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<TourDto> CreateAsync(TourCreateDto dto, CancellationToken cancellationToken);
    Task UpdateAsync(int id, TourUpdateDto dto, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<TourDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken);
}
