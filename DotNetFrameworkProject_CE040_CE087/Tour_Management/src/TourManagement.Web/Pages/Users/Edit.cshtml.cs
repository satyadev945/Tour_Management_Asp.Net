using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Contracts;
using TourManagement.Application.DTOs;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

public sealed class EditModel : PageModel
{
    private readonly IApplicationUserService _userService;
    public EditModel(IApplicationUserService userService) => _userService = userService;
    [BindProperty] public UserEditViewModel EditUser { get; set; } = new();
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var dto = await _userService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();
        EditUser = new UserEditViewModel { FirstName = dto.FirstName, LastName = dto.LastName, Gender = dto.Gender, DateOfBirth = dto.DateOfBirth, Street = dto.Street, City = dto.City, State = dto.State };
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return Page();
        await _userService.UpdateAsync(id, new ApplicationUserUpdateDto { FirstName = EditUser.FirstName, LastName = EditUser.LastName, Gender = EditUser.Gender, DateOfBirth = EditUser.DateOfBirth, Street = EditUser.Street, City = EditUser.City, State = EditUser.State }, cancellationToken);
        return RedirectToPage("Index");
    }
}
