using Microsoft.AspNetCore.Mvc;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Controllers;

/// <summary>
/// Controller for Booking management operations
/// </summary>
public class BookingController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly ITourService _tourService;
    private readonly ILogger<BookingController> _logger;

    public BookingController(
        IBookingService bookingService,
        ITourService tourService,
        ILogger<BookingController> logger)
    {
        _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        _tourService = tourService ?? throw new ArgumentNullException(nameof(tourService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Booking
    public async Task<IActionResult> Index()
    {
        try
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Admin")
            {
                var allBookings = await _bookingService.GetAllBookingsAsync();
                return View(allBookings);
            }
            else
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    return RedirectToAction("Login", "User");
                }

                var userBookings = await _bookingService.GetBookingsByUserIdAsync(userId.Value);
                return View(userBookings);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings");
            TempData["Error"] = "Error loading bookings";
            return View(new List<BookingDto>());
        }
    }

    // GET: Booking/MyBookings
    public async Task<IActionResult> MyBookings()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId.Value);
            return View(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user bookings");
            TempData["Error"] = "Error loading your bookings";
            return View(new List<BookingDto>());
        }
    }

    // GET: Booking/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Check if user has permission to view this booking
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (userRole != "Admin" && booking.UserId != userId)
            {
                return Forbid();
            }

            return View(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking details for ID: {BookingId}", id);
            return NotFound();
        }
    }

    // GET: Booking/Create
    public async Task<IActionResult> Create(int? tourId)
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (tourId.HasValue)
            {
                var tour = await _tourService.GetTourByIdAsync(tourId.Value);
                if (tour != null)
                {
                    ViewBag.Tour = tour;
                }
            }

            var createDto = new CreateBookingDto
            {
                UserId = userId.Value,
                TourId = tourId ?? 0,
                BookingDate = DateTime.Now.AddDays(7),
                NumberOfPeople = 1
            };

            return View(createDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking create page");
            return RedirectToAction("Display", "Tour");
        }
    }

    // POST: Booking/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingDto createBookingDto)
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            createBookingDto.UserId = userId.Value;

            if (!ModelState.IsValid)
            {
                var tour = await _tourService.GetTourByIdAsync(createBookingDto.TourId);
                if (tour != null)
                {
                    ViewBag.Tour = tour;
                }
                return View(createBookingDto);
            }

            await _bookingService.CreateBookingAsync(createBookingDto);
            TempData["Success"] = "Booking created successfully";
            return RedirectToAction(nameof(MyBookings));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            TempData["Error"] = "Error creating booking: " + ex.Message;
            return View(createBookingDto);
        }
    }

    // POST: Booking/Cancel/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Check if user has permission to cancel this booking
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (userRole != "Admin" && booking.UserId != userId)
            {
                return Forbid();
            }

            var result = await _bookingService.CancelBookingAsync(id);
            if (!result)
            {
                return NotFound();
            }

            TempData["Success"] = "Booking cancelled successfully";
            return RedirectToAction(nameof(MyBookings));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking, ID: {BookingId}", id);
            TempData["Error"] = "Error cancelling booking";
            return RedirectToAction(nameof(MyBookings));
        }
    }

    // POST: Booking/UpdateStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        try
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Forbid();
            }

            var updateDto = new UpdateBookingStatusDto
            {
                Id = id,
                Status = status
            };

            var result = await _bookingService.UpdateBookingStatusAsync(updateDto);
            if (result == null)
            {
                return NotFound();
            }

            TempData["Success"] = "Booking status updated successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking status, ID: {BookingId}", id);
            TempData["Error"] = "Error updating booking status";
            return RedirectToAction(nameof(Index));
        }
    }
}
