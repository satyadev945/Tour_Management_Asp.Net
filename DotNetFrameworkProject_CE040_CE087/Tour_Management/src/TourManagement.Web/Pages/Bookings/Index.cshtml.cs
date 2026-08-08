using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Contracts;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

public sealed class IndexModel : PageModel
{
    private readonly IBookingService _bookingService;
    public IndexModel(IBookingService bookingService) => _bookingService = bookingService;
    public IReadOnlyList<BookingListItemViewModel> Bookings { get; private set; } = [];
    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var bookings = await _bookingService.GetAllAsync(cancellationToken);
        Bookings = bookings.Select(x => new BookingListItemViewModel { Id = x.Id, TourName = x.TourName, CustomerName = x.CustomerName, Email = x.Email, City = x.City, PhoneNumber = x.PhoneNumber, BookingDate = x.BookingDate }).ToList();
    }
}
