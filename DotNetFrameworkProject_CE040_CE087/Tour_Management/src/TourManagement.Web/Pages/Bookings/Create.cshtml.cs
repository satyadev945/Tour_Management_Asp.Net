using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TourManagement.Application.DTOs;
using TourManagement.Application.Contracts;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

public sealed class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ITourService _tourService;
    public CreateModel(IBookingService bookingService, ITourService tourService) { _bookingService = bookingService; _tourService = tourService; }
    [BindProperty] public BookingFormViewModel Booking { get; set; } = new();
    public List<SelectListItem> TourOptions { get; private set; } = [];
    public async Task OnGetAsync(int? tourId, CancellationToken cancellationToken)
    {
        await LoadToursAsync(cancellationToken);
        if (tourId.HasValue) Booking.TourId = tourId.Value;
    }
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        await LoadToursAsync(cancellationToken);
        if (!ModelState.IsValid) return Page();
        await _bookingService.CreateAsync(new BookingCreateDto { TourId = Booking.TourId, CustomerName = Booking.CustomerName, Email = Booking.Email, City = Booking.City, PhoneNumber = Booking.PhoneNumber }, cancellationToken);
        return RedirectToPage("Index");
    }
    private async Task LoadToursAsync(CancellationToken cancellationToken)
    {
        TourOptions = (await _tourService.GetAllAsync(cancellationToken)).Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
    }
}
