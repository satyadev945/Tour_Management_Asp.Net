using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Contracts;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

public sealed class IndexModel : PageModel
{
    private readonly ITourService _tourService;

    public IndexModel(ITourService tourService) => _tourService = tourService;

    public IReadOnlyList<TourListItemViewModel> Tours { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var tours = await _tourService.GetAllAsync(cancellationToken);
        Tours = tours.Select(x => new TourListItemViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Place = x.Place,
            Days = x.Days,
            Price = x.Price,
            Locations = x.Locations,
            Description = x.Description,
            ImagePath = x.ImagePath
        }).ToList();
    }
}
