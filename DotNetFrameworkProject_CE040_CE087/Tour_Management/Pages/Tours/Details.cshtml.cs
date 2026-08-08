using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Tours;

public class DetailsModel : PageModel
{
    private readonly ITourService _tourService;

    public DetailsModel(ITourService tourService)
    {
        _tourService = tourService;
    }

    public TourDto? Tour { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Tour = await _tourService.GetByIdAsync(id, cancellationToken);
        return Page();
    }
}
