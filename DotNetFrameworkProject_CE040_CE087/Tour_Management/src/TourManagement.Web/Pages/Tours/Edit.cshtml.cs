using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Contracts;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

public sealed class EditModel : PageModel
{
    private readonly ITourService _tourService;
    public EditModel(ITourService tourService) => _tourService = tourService;
    [BindProperty] public TourFormViewModel Tour { get; set; } = new();
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var dto = await _tourService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();
        Tour = new TourFormViewModel { Name = dto.Name, Place = dto.Place, Days = dto.Days, Price = dto.Price, Locations = dto.Locations, Description = dto.Description, ImagePath = dto.ImagePath };
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return Page();
        await _tourService.UpdateAsync(id, new TourUpdateDto { Name = Tour.Name, Place = Tour.Place, Days = Tour.Days, Price = Tour.Price, Locations = Tour.Locations, Description = Tour.Description, ImagePath = Tour.ImagePath }, cancellationToken);
        return RedirectToPage("Index");
    }
}
