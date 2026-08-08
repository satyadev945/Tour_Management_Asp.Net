using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Tours;

public class EditModel : PageModel
{
    private readonly ITourService _tourService;

    public EditModel(ITourService tourService)
    {
        _tourService = tourService;
    }

    [BindProperty]
    public TourInputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var tour = await _tourService.GetByIdAsync(id, cancellationToken);
        if (tour is null)
        {
            return NotFound();
        }

        Input = new TourInputModel
        {
            Id = tour.Id,
            Name = tour.Name,
            Place = tour.Place,
            Days = tour.Days,
            Price = tour.Price,
            Locations = tour.Locations,
            Description = tour.Description,
            ImagePath = tour.ImagePath
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _tourService.UpdateAsync(Input.Id, new TourUpdateDto
        {
            Name = Input.Name,
            Place = Input.Place,
            Days = Input.Days,
            Price = Input.Price,
            Locations = Input.Locations,
            Description = Input.Description,
            ImagePath = Input.ImagePath
        }, cancellationToken);

        return RedirectToPage("Index");
    }

    public class TourInputModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Place { get; set; } = string.Empty;
        [Range(1, 365)]
        public int Days { get; set; }
        [Range(0, 100000)]
        public decimal Price { get; set; }
        [Required]
        public string Locations { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
    }
}
