using Microsoft.AspNetCore.Mvc;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Controllers;

/// <summary>
/// Controller for User management operations
/// </summary>
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: User/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: User/Login
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
            
            if (result.Success && result.User != null)
            {
                // Store user info in session
                HttpContext.Session.SetInt32("UserId", result.User.Id);
                HttpContext.Session.SetString("UserEmail", result.User.Email);
                HttpContext.Session.SetString("UserRole", result.User.Role);
                
                TempData["Success"] = "Login successful";
                
                // Redirect based on role
                if (result.User.Role == "Admin")
                {
                    return RedirectToAction("Profile", "Admin");
                }
                else
                {
                    return RedirectToAction("Profile", "User");
                }
            }
            else
            {
                TempData["Error"] = result.Message;
                return View(loginDto);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            TempData["Error"] = "An error occurred during login";
            return View(loginDto);
        }
    }

    // GET: User/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: User/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUserDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            var user = await _userService.RegisterUserAsync(registerDto);
            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction(nameof(Login));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Registration failed");
            TempData["Error"] = ex.Message;
            return View(registerDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            TempData["Error"] = "An error occurred during registration";
            return View(registerDto);
        }
    }

    // GET: User/Profile
    public async Task<IActionResult> Profile()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user profile");
            return RedirectToAction(nameof(Login));
        }
    }

    // GET: User/Edit
    public async Task<IActionResult> Edit()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user for edit");
            return RedirectToAction(nameof(Profile));
        }
    }

    // POST: User/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserDto userDto)
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId.Value != userDto.Id)
            {
                return RedirectToAction(nameof(Login));
            }

            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

            var result = await _userService.UpdateUserAsync(userDto.Id, userDto);
            if (result == null)
            {
                return NotFound();
            }

            TempData["Success"] = "Profile updated successfully";
            return RedirectToAction(nameof(Profile));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile");
            TempData["Error"] = "Error updating profile";
            return View(userDto);
        }
    }

    // GET: User/Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        TempData["Success"] = "Logged out successfully";
        return RedirectToAction("Index", "Home");
    }

    // GET: User/Index (Admin only)
    public async Task<IActionResult> Index()
    {
        try
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Forbid();
            }

            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users");
            TempData["Error"] = "Error loading users";
            return View(new List<UserDto>());
        }
    }

    // POST: User/Delete/5 (Admin only)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Forbid();
            }

            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }

            TempData["Success"] = "User deleted successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user, ID: {UserId}", id);
            TempData["Error"] = "Error deleting user";
            return RedirectToAction(nameof(Index));
        }
    }
}
