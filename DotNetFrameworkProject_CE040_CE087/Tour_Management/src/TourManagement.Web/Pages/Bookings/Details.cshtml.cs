using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Contracts;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

public sealed class DetailsModel : PageModel
{
    private readonly IBookingService _bookingService;
    public DetailsModel(IBookingService bookingService) => _bookingService = bookingService;
    public BookingListItemViewModel? Booking { get; private set; }
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var dto = await _bookingService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();
        Booking = new BookingListItemViewModel { Id = dto.Id, TourName = dto.TourName, CustomerName = dto.CustomerName, Email = dto.Email, City = dto.City, PhoneNumber = dto.PhoneNumber, BookingDate = dto.BookingDate };
        return Page();
    }
}
