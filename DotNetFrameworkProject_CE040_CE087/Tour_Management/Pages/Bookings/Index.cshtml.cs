using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Bookings;

public class IndexModel : PageModel
{
    private readonly IBookingService _bookingService;

    public IndexModel(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public IReadOnlyList<BookingDto> Bookings { get; private set; } = Array.Empty<BookingDto>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Bookings = await _bookingService.GetAllAsync(cancellationToken);
    }
}
