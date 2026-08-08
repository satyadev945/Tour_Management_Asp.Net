using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Tours;

public class CreateModel : PageModel
{
    private readonly ITourService _tourService;

    public CreateModel(ITourService tourService)
    {
        _tourService = tourService;
    }

    [BindProperty]
    public TourInputModel Input { get; set; } = new();

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _tourService.CreateAsync(new TourCreateDto
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
