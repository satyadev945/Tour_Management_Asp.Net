using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Tours;

public class DeleteModel : PageModel
{
    private readonly ITourService _tourService;

    public DeleteModel(ITourService tourService)
    {
        _tourService = tourService;
    }

    [BindProperty]
    public TourDto? Tour { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Tour = await _tourService.GetByIdAsync(id, cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (Tour is null)
        {
            return RedirectToPage("Index");
        }

        await _tourService.DeleteAsync(Tour.Id, cancellationToken);
        return RedirectToPage("Index");
    }
}
