using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Data;
using TourManagement.Infrastructure.Interfaces;

namespace TourManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Booking entity
/// </summary>
public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(TourManagementDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId)
    {
        return await _dbSet
            .Where(b => b.UserId == userId)
            .Include(b => b.Tour)
            .Include(b => b.User)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByTourIdAsync(int tourId)
    {
        return await _dbSet
            .Where(b => b.TourId == tourId)
            .Include(b => b.Tour)
            .Include(b => b.User)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _dbSet
            .Include(b => b.Tour)
            .Include(b => b.User)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    public override async Task<Booking?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(b => b.Tour)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}
