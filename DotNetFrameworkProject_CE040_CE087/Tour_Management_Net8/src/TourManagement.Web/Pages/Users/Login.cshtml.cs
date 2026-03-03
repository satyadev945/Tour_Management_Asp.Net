using TourManagement.Domain.DTOs;
namespace TourManagement.Web.Pages.Users;

public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    public UserLoginViewModel LoginViewModel { get; set; } = new UserLoginViewModel();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Manual mapping from ViewModel to DTO
            var loginDto = new UserLoginDto
            {
                Email = LoginViewModel.Email,
                Password = LoginViewModel.Password
            };

            var user = await _userService.ValidateLoginAsync(loginDto);

            if (user != null)
            {
                // Store user information in session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserEmail", user.Email);

                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            ModelState.AddModelError(string.Empty, "An error occurred during login.");
            return Page();
        }
    }
}
