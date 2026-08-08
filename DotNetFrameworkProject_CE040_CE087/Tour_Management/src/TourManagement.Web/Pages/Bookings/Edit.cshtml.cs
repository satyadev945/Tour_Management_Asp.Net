using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TourManagement.Application.DTOs;
using TourManagement.Application.Contracts;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

public sealed class EditModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ITourService _tourService;
    public EditModel(IBookingService bookingService, ITourService tourService) { _bookingService = bookingService; _tourService = tourService; }
    [BindProperty] public BookingFormViewModel Booking { get; set; } = new();
    public List<SelectListItem> TourOptions { get; private set; } = [];
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        await LoadToursAsync(cancellationToken);
        var dto = await _bookingService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();
        Booking = new BookingFormViewModel { TourId = dto.TourId, CustomerName = dto.CustomerName, Email = dto.Email, City = dto.City, PhoneNumber = dto.PhoneNumber };
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await LoadToursAsync(cancellationToken);
        if (!ModelState.IsValid) return Page();
        await _bookingService.UpdateAsync(id, new BookingUpdateDto { TourId = Booking.TourId, CustomerName = Booking.CustomerName, Email = Booking.Email, City = Booking.City, PhoneNumber = Booking.PhoneNumber }, cancellationToken);
        return RedirectToPage("Index");
    }
    private async Task LoadToursAsync(CancellationToken cancellationToken)
    {
        TourOptions = (await _tourService.GetAllAsync(cancellationToken)).Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
    }
}
