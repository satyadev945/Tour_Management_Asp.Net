using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Contracts;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

public sealed class CreateModel : PageModel
{
    private readonly ITourService _tourService;

    public CreateModel(ITourService tourService) => _tourService = tourService;

    [BindProperty]
    public TourFormViewModel Tour { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _tourService.CreateAsync(new TourCreateDto
        {
            Name = Tour.Name,
            Place = Tour.Place,
            Days = Tour.Days,
            Price = Tour.Price,
            Locations = Tour.Locations,
            Description = Tour.Description,
            ImagePath = Tour.ImagePath
        }, cancellationToken);

        return RedirectToPage("Index");
    }
}
