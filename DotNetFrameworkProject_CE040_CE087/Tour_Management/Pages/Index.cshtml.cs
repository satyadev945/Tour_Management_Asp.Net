using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages;

public class IndexModel : PageModel
{
    private readonly ITourService _tourService;

    public IndexModel(ITourService tourService)
    {
        _tourService = tourService;
    }

    public IReadOnlyList<TourDto> FeaturedTours { get; private set; } = Array.Empty<TourDto>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        FeaturedTours = (await _tourService.GetAllAsync(cancellationToken)).Take(6).ToList();
    }
}
