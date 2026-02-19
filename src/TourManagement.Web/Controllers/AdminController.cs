using Microsoft.AspNetCore.Mvc;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Controllers;

/// <summary>
/// Controller for Admin operations
/// </summary>
public class AdminController : Controller
{
    private readonly IUserService _userService;
    private readonly ITourService _tourService;
    private readonly IBookingService _bookingService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        IUserService userService,
        ITourService tourService,
        IBookingService bookingService,
        ILogger<AdminController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _tourService = tourService ?? throw new ArgumentNullException(nameof(tourService));
        _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Admin/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var result = await _userService.LoginAsync(loginDto);
            
            if (result.Success && result.User != null && result.User.Role == "Admin")
            {
                // Store admin info in session
                HttpContext.Session.SetInt32("UserId", result.User.Id);
                HttpContext.Session.SetString("UserEmail", result.User.Email);
                HttpContext.Session.SetString("UserRole", result.User.Role);
                
                TempData["Success"] = "Admin login successful";
                return RedirectToAction(nameof(Profile));
            }
            else
            {
                TempData["Error"] = "Invalid admin credentials";
                return View(loginDto);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during admin login");
            TempData["Error"] = "An error occurred during login";
            return View(loginDto);
        }
    }

    // GET: Admin/Profile
    public async Task<IActionResult> Profile()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (userId == null || userRole != "Admin")
            {
                return RedirectToAction(nameof(Login));
            }

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            // Get statistics for dashboard
            var allTours = await _tourService.GetAllToursAsync();
            var allBookings = await _bookingService.GetAllBookingsAsync();
            var allUsers = await _userService.GetAllUsersAsync();

            ViewBag.TotalTours = allTours.Count();
            ViewBag.TotalBookings = allBookings.Count();
            ViewBag.TotalUsers = allUsers.Count();
            ViewBag.PendingBookings = allBookings.Count(b => b.Status == "Pending");

            return View(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin profile");
            return RedirectToAction(nameof(Login));
        }
    }

    // GET: Admin/Dashboard
    public async Task<IActionResult> Dashboard()
    {
        try
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction(nameof(Login));
            }

            var allTours = await _tourService.GetAllToursAsync();
            var allBookings = await _bookingService.GetAllBookingsAsync();
            var allUsers = await _userService.GetAllUsersAsync();

            ViewBag.TotalTours = allTours.Count();
            ViewBag.ActiveTours = allTours.Count(t => t.IsActive);
            ViewBag.TotalBookings = allBookings.Count();
            ViewBag.PendingBookings = allBookings.Count(b => b.Status == "Pending");
            ViewBag.ConfirmedBookings = allBookings.Count(b => b.Status == "Confirmed");
            ViewBag.CancelledBookings = allBookings.Count(b => b.Status == "Cancelled");
            ViewBag.TotalUsers = allUsers.Count();
            ViewBag.ActiveUsers = allUsers.Count(u => u.IsActive);

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin dashboard");
            TempData["Error"] = "Error loading dashboard";
            return View();
        }
    }
}
