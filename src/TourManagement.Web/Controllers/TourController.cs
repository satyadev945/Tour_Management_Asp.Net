using Microsoft.AspNetCore.Mvc;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Controllers;

/// <summary>
/// Controller for Tour management operations
/// </summary>
public class TourController : Controller
{
    private readonly ITourService _tourService;
    private readonly ILogger<TourController> _logger;
    private readonly IWebHostEnvironment _environment;

    public TourController(
        ITourService tourService,
        ILogger<TourController> logger,
        IWebHostEnvironment environment)
    {
        _tourService = tourService ?? throw new ArgumentNullException(nameof(tourService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    // GET: Tour
    public async Task<IActionResult> Index()
    {
        try
        {
            var tours = await _tourService.GetAllToursAsync();
            return View(tours);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours");
            TempData["Error"] = "Error loading tours";
            return View(new List<TourDto>());
        }
    }

    // GET: Tour/Display
    public async Task<IActionResult> Display()
    {
        try
        {
            var tours = await _tourService.GetActiveToursAsync();
            return View(tours);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error displaying tours");
            TempData["Error"] = "Error displaying tours";
            return View(new List<TourDto>());
        }
    }

    // GET: Tour/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            return View(tour);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour details for ID: {TourId}", id);
            return NotFound();
        }
    }

    // GET: Tour/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Tour/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTourDto createTourDto, IFormFile? pictureFile)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(createTourDto);
            }

            // Handle file upload
            if (pictureFile != null && pictureFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Tour_pics");
                Directory.CreateDirectory(uploadsFolder);
                
                var uniqueFileName = $"{Guid.NewGuid()}_{pictureFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await pictureFile.CopyToAsync(fileStream);
                }
                
                createTourDto.PicturePath = $"/Tour_pics/{uniqueFileName}";
            }

            await _tourService.CreateTourAsync(createTourDto);
            TempData["Success"] = "Tour created successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour");
            TempData["Error"] = "Error creating tour";
            return View(createTourDto);
        }
    }

    // GET: Tour/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateTourDto
            {
                Id = tour.Id,
                TourName = tour.TourName,
                Place = tour.Place,
                Days = tour.Days,
                Price = tour.Price,
                Locations = tour.Locations,
                TourInfo = tour.TourInfo,
                PicturePath = tour.PicturePath,
                IsActive = tour.IsActive
            };

            return View(updateDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for edit, ID: {TourId}", id);
            return NotFound();
        }
    }

    // POST: Tour/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateTourDto updateTourDto, IFormFile? pictureFile)
    {
        try
        {
            if (id != updateTourDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(updateTourDto);
            }

            // Handle file upload
            if (pictureFile != null && pictureFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Tour_pics");
                Directory.CreateDirectory(uploadsFolder);
                
                var uniqueFileName = $"{Guid.NewGuid()}_{pictureFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await pictureFile.CopyToAsync(fileStream);
                }
                
                updateTourDto.PicturePath = $"/Tour_pics/{uniqueFileName}";
            }

            var result = await _tourService.UpdateTourAsync(updateTourDto);
            if (result == null)
            {
                return NotFound();
            }

            TempData["Success"] = "Tour updated successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour, ID: {TourId}", id);
            TempData["Error"] = "Error updating tour";
            return View(updateTourDto);
        }
    }

    // GET: Tour/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            return View(tour);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for delete, ID: {TourId}", id);
            return NotFound();
        }
    }

    // POST: Tour/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var result = await _tourService.DeleteTourAsync(id);
            if (!result)
            {
                return NotFound();
            }

            TempData["Success"] = "Tour deleted successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour, ID: {TourId}", id);
            TempData["Error"] = "Error deleting tour";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Tour/Search
    public async Task<IActionResult> Search(string searchTerm)
    {
        try
        {
            var tours = await _tourService.SearchToursAsync(searchTerm ?? string.Empty);
            ViewBag.SearchTerm = searchTerm;
            return View("Index", tours);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching tours with term: {SearchTerm}", searchTerm);
            TempData["Error"] = "Error searching tours";
            return View("Index", new List<TourDto>());
        }
    }
}
