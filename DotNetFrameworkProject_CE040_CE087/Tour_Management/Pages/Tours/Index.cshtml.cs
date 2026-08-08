using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Tours;

public class IndexModel : PageModel
{
    private readonly ITourService _tourService;

    public IndexModel(ITourService tourService)
    {
        _tourService = tourService;
    }

    public IReadOnlyList<TourDto> Tours { get; private set; } = Array.Empty<TourDto>();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Tours = string.IsNullOrWhiteSpace(SearchTerm)
            ? await _tourService.GetAllAsync(cancellationToken)
            : await _tourService.SearchAsync(SearchTerm, cancellationToken);
    }
}
