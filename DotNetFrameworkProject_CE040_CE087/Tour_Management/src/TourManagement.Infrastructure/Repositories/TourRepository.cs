using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Infrastructure.Data;

namespace TourManagement.Infrastructure.Repositories;

public sealed class TourRepository : ITourRepository
{
    private readonly TourManagementDbContext _context;

    public TourRepository(TourManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Tour>> GetAllAsync(CancellationToken cancellationToken) =>
        await _context.Tours.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Name).ToListAsync(cancellationToken);

    public Task<Tour?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        _context.Tours.Include(x => x.Bookings).FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

    public async Task<Tour> AddAsync(Tour entity, CancellationToken cancellationToken)
    {
        _context.Tours.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Tour entity, CancellationToken cancellationToken)
    {
        _context.Tours.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Tours.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new EntityNotFoundException($"Tour {id} was not found.");
        _context.Tours.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) =>
        _context.Tours.AnyAsync(x => x.Id == id && x.IsActive, cancellationToken);

    public async Task<IReadOnlyList<Tour>> SearchAsync(string searchTerm, CancellationToken cancellationToken) =>
        await _context.Tours.AsNoTracking()
            .Where(x => x.IsActive && (x.Name.Contains(searchTerm) || x.Place.Contains(searchTerm) || x.Locations.Contains(searchTerm)))
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
}
