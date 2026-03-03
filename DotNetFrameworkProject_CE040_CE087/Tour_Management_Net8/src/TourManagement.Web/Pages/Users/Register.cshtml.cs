using TourManagement.Domain.DTOs;
namespace TourManagement.Web.Pages.Users;

public class RegisterModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<RegisterModel> _logger;

    public RegisterModel(IUserService userService, ILogger<RegisterModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    public UserRegisterViewModel RegisterViewModel { get; set; } = new UserRegisterViewModel();

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
            var createDto = new UserCreateDto
            {
                Email = RegisterViewModel.Email,
                Password = RegisterViewModel.Password,
                FirstName = RegisterViewModel.FirstName,
                LastName = RegisterViewModel.LastName,
                PhoneNumber = RegisterViewModel.PhoneNumber
            };

            await _userService.CreateAsync(createDto);

            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToPage("./Login");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Registration failed - email already exists");
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            ModelState.AddModelError(string.Empty, "An error occurred during registration.");
            return Page();
        }
    }
}
