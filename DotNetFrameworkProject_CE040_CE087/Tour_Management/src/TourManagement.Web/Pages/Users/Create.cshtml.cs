using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Contracts;
using TourManagement.Application.DTOs;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

public sealed class CreateModel : PageModel
{
    private readonly IApplicationUserService _userService;
    public CreateModel(IApplicationUserService userService) => _userService = userService;
    [BindProperty] public UserRegistrationViewModel Registration { get; set; } = new();
    public void OnGet() { }
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return Page();
        await _userService.CreateAsync(new ApplicationUserCreateDto { Email = Registration.Email, FirstName = Registration.FirstName, LastName = Registration.LastName, Gender = Registration.Gender, DateOfBirth = Registration.DateOfBirth, Street = Registration.Street, City = Registration.City, State = Registration.State, Password = Registration.Password }, cancellationToken);
        return RedirectToPage("Index");
    }
}
