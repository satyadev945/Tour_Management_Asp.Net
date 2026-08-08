using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Bookings;

public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ITourService _tourService;

    public CreateModel(IBookingService bookingService, ITourService tourService)
    {
        _bookingService = bookingService;
        _tourService = tourService;
    }

    [BindProperty]
    public BookingInputModel Input { get; set; } = new();

    public List<SelectListItem> TourOptions { get; private set; } = new();

    public async Task OnGetAsync(int? tourId, CancellationToken cancellationToken)
    {
        await LoadToursAsync(cancellationToken);
        if (tourId.HasValue)
        {
            Input.TourId = tourId.Value;
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        await LoadToursAsync(cancellationToken);
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _bookingService.CreateAsync(new BookingCreateDto
        {
            TourId = Input.TourId,
            CustomerName = Input.CustomerName,
            Email = Input.Email,
            Place = Input.Place
        }, cancellationToken);

        return RedirectToPage("Index");
    }

    private async Task LoadToursAsync(CancellationToken cancellationToken)
    {
        TourOptions = (await _tourService.GetAllAsync(cancellationToken))
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
    }

    public class BookingInputModel
    {
        [Display(Name = "Tour")]
        public int TourId { get; set; }
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Place { get; set; } = string.Empty;
    }
}
