using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Interfaces;

/// <summary>
/// Repository interface for Tour entity
/// </summary>
public interface ITourRepository : IRepository<Tour>
{
    Task<IEnumerable<Tour>> GetActiveToursAsync();
    Task<IEnumerable<Tour>> SearchToursAsync(string searchTerm);
}
