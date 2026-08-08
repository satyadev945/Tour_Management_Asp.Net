using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Bookings;

public class DeleteModel : PageModel
{
    private readonly IBookingService _bookingService;

    public DeleteModel(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [BindProperty]
    public BookingDto? Booking { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Booking = (await _bookingService.GetAllAsync(cancellationToken)).FirstOrDefault(x => x.Id == id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (Booking is not null)
        {
            await _bookingService.DeleteAsync(Booking.Id, cancellationToken);
        }

        return RedirectToPage("Index");
    }
}
