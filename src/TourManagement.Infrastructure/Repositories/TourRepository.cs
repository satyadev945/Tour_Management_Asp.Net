using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Data;
using TourManagement.Infrastructure.Interfaces;

namespace TourManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Tour entity
/// </summary>
public class TourRepository : Repository<Tour>, ITourRepository
{
    public TourRepository(TourManagementDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Tour>> GetActiveToursAsync()
    {
        return await _dbSet
            .Where(t => t.IsActive)
            .OrderBy(t => t.TourName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tour>> SearchToursAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAllAsync();
        }

        return await _dbSet
            .Where(t => t.IsActive &&
                       (t.TourName.Contains(searchTerm) ||
                        t.Place.Contains(searchTerm) ||
                        t.Locations.Contains(searchTerm) ||
                        t.TourInfo.Contains(searchTerm)))
            .OrderBy(t => t.TourName)
            .ToListAsync();
    }
}
