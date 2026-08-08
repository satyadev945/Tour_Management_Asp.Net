using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Infrastructure.Data;

namespace TourManagement.Infrastructure.Repositories;

public sealed class ApplicationUserRepository : IApplicationUserRepository
{
    private readonly TourManagementDbContext _context;

    public ApplicationUserRepository(TourManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken) =>
        await _context.Users.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Email).ToListAsync(cancellationToken);

    public Task<ApplicationUser?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

    public Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsActive, cancellationToken);

    public async Task<ApplicationUser> AddAsync(ApplicationUser entity, CancellationToken cancellationToken)
    {
        _context.Users.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(ApplicationUser entity, CancellationToken cancellationToken)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new EntityNotFoundException($"User {id} was not found.");
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) =>
        _context.Users.AnyAsync(x => x.Id == id && x.IsActive, cancellationToken);

    public async Task<IReadOnlyList<ApplicationUser>> SearchAsync(string searchTerm, CancellationToken cancellationToken) =>
        await _context.Users.AsNoTracking()
            .Where(x => x.IsActive && (x.Email.Contains(searchTerm) || x.FirstName.Contains(searchTerm) || x.LastName.Contains(searchTerm) || x.City.Contains(searchTerm)))
            .OrderBy(x => x.Email)
            .ToListAsync(cancellationToken);
}
