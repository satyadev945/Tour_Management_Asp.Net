using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Repositories;
using Tour_Management.Infrastructure.Data;

namespace Tour_Management.Infrastructure.Repositories;

public class TourRepository : ITourRepository
{
    private readonly TourManagementDbContext _context;
    private readonly ILogger<TourRepository> _logger;

    public TourRepository(TourManagementDbContext context, ILogger<TourRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Tour>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Tours.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Name).ToListAsync(cancellationToken);

    public async Task<Tour?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await _context.Tours.FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

    public async Task<Tour> AddAsync(Tour tour, CancellationToken cancellationToken)
    {
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync(cancellationToken);
        return tour;
    }

    public async Task UpdateAsync(Tour tour, CancellationToken cancellationToken)
    {
        _context.Tours.Update(tour);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Tours.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
        {
            _logger.LogWarning("Tour {TourId} was not found for deletion", id);
            return;
        }

        entity.IsActive = false;
        entity.ModifiedDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        => await _context.Tours.AnyAsync(x => x.Id == id && x.IsActive, cancellationToken);

    public async Task<IReadOnlyList<Tour>> SearchAsync(string searchTerm, CancellationToken cancellationToken)
        => await _context.Tours.AsNoTracking()
            .Where(x => x.IsActive && (x.Name.Contains(searchTerm) || x.Place.Contains(searchTerm) || x.Locations.Contains(searchTerm)))
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
}
