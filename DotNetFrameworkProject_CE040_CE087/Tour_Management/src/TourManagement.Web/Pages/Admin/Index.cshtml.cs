using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Contracts;

namespace TourManagement.Web.Pages.Admin;

public sealed class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IBookingService _bookingService;
    private readonly IApplicationUserService _userService;

    public IndexModel(ITourService tourService, IBookingService bookingService, IApplicationUserService userService)
    {
        _tourService = tourService;
        _bookingService = bookingService;
        _userService = userService;
    }

    public int TourCount { get; private set; }
    public int BookingCount { get; private set; }
    public int UserCount { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        TourCount = (await _tourService.GetAllAsync(cancellationToken)).Count;
        BookingCount = (await _bookingService.GetAllAsync(cancellationToken)).Count;
        UserCount = (await _userService.GetAllAsync(cancellationToken)).Count;
    }
}
