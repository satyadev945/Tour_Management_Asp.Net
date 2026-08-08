using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Bookings;

public class DetailsModel : PageModel
{
    private readonly IBookingService _bookingService;

    public DetailsModel(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public BookingDto? Booking { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Booking = (await _bookingService.GetAllAsync(cancellationToken)).FirstOrDefault(x => x.Id == id);
        return Page();
    }
}
