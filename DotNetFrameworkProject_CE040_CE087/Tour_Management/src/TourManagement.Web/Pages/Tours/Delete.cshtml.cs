using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Contracts;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

public sealed class DeleteModel : PageModel
{
    private readonly ITourService _tourService;
    public DeleteModel(ITourService tourService) => _tourService = tourService;
    public TourListItemViewModel? Tour { get; private set; }
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var dto = await _tourService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();
        Tour = new TourListItemViewModel { Id = dto.Id, Name = dto.Name, Place = dto.Place, Days = dto.Days, Price = dto.Price, Locations = dto.Locations, Description = dto.Description, ImagePath = dto.ImagePath };
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await _tourService.DeleteAsync(id, cancellationToken);
        return RedirectToPage("Index");
    }
}
