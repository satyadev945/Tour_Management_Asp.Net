using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IBookingService _bookingService;

    public IndexModel(ITourService tourService, IBookingService bookingService)
    {
        _tourService = tourService;
        _bookingService = bookingService;
    }

    public int TotalTours { get; private set; }
    public int TotalBookings { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        TotalTours = (await _tourService.GetAllAsync(cancellationToken)).Count;
        TotalBookings = (await _bookingService.GetAllAsync(cancellationToken)).Count;
    }
}
