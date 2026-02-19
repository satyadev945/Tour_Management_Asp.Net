using Microsoft.AspNetCore.Mvc;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Controllers;

/// <summary>
/// Home controller for main pages
/// </summary>
public class HomeController : Controller
{
    private readonly ITourService _tourService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ITourService tourService, ILogger<HomeController> logger)
    {
        _tourService = tourService ?? throw new ArgumentNullException(nameof(tourService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var tours = await _tourService.GetActiveToursAsync();
            return View(tours);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading home page");
            return View("Error");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
