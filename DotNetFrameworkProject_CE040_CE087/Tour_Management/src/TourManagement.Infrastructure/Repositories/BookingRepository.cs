using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Infrastructure.Data;

namespace TourManagement.Infrastructure.Repositories;

public sealed class BookingRepository : IBookingRepository
{
    private readonly TourManagementDbContext _context;

    public BookingRepository(TourManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken) =>
        await _context.Bookings.AsNoTracking().Include(x => x.Tour).Where(x => x.IsActive).OrderByDescending(x => x.BookingDate).ToListAsync(cancellationToken);

    public Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        _context.Bookings.Include(x => x.Tour).FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

    public async Task<Booking> AddAsync(Booking entity, CancellationToken cancellationToken)
    {
        _context.Bookings.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Booking entity, CancellationToken cancellationToken)
    {
        _context.Bookings.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new EntityNotFoundException($"Booking {id} was not found.");
        _context.Bookings.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) =>
        _context.Bookings.AnyAsync(x => x.Id == id && x.IsActive, cancellationToken);

    public async Task<IReadOnlyList<Booking>> SearchAsync(string searchTerm, CancellationToken cancellationToken) =>
        await _context.Bookings.AsNoTracking().Include(x => x.Tour)
            .Where(x => x.IsActive && (x.CustomerName.Contains(searchTerm) || x.Email.Contains(searchTerm) || x.City.Contains(searchTerm)))
            .OrderByDescending(x => x.BookingDate)
            .ToListAsync(cancellationToken);
}
