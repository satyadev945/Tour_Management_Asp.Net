using TourManagement.Domain.DTOs;
namespace TourManagement.Web.Pages.Tours;

public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    public IEnumerable<TourViewModel> Tours { get; set; } = new List<TourViewModel>();
    
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            IEnumerable<TourDto> tourDtos;
            
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                tourDtos = await _tourService.SearchAsync(SearchTerm);
            }
            else
            {
                tourDtos = await _tourService.GetAllAsync();
            }

            // Manual mapping from DTO to ViewModel
            Tours = tourDtos.Select(dto => new TourViewModel
            {
                Id = dto.Id,
                TourName = dto.TourName,
                Place = dto.Place,
                Days = dto.Days,
                Price = dto.Price,
                Locations = dto.Locations,
                TourInfo = dto.TourInfo,
                PictureFileName = dto.PictureFileName
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours");
            Tours = new List<TourViewModel>();
        }
    }
}
