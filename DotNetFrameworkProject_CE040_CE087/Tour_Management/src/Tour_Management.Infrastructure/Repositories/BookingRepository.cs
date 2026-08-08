using Microsoft.EntityFrameworkCore;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Repositories;
using Tour_Management.Infrastructure.Data;

namespace Tour_Management.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly TourManagementDbContext _context;

    public BookingRepository(TourManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Bookings.AsNoTracking().Include(x => x.Tour).OrderByDescending(x => x.BookingDate).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Booking>> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => await _context.Bookings.AsNoTracking().Include(x => x.Tour).Where(x => x.Email == email).OrderByDescending(x => x.BookingDate).ToListAsync(cancellationToken);

    public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await _context.Bookings.Include(x => x.Tour).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Booking> AddAsync(Booking booking, CancellationToken cancellationToken)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(booking.Id, cancellationToken) ?? booking;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
        {
            return;
        }

        _context.Bookings.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
