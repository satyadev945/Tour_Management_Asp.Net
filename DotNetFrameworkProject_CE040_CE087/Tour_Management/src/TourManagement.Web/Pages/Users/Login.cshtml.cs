using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Contracts;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

public sealed class LoginModel : PageModel
{
    private readonly IApplicationUserService _userService;
    public LoginModel(IApplicationUserService userService) => _userService = userService;
    [BindProperty] public LoginViewModel Login { get; set; } = new();
    public string? Message { get; private set; }
    public void OnGet() { }
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return Page();
        var valid = await _userService.ValidateCredentialsAsync(Login.Email, Login.Password, cancellationToken);
        if (!valid)
        {
            Message = "Invalid email or password.";
            return Page();
        }
        TempData["Message"] = "Login successful.";
        return RedirectToPage("Index");
    }
}
