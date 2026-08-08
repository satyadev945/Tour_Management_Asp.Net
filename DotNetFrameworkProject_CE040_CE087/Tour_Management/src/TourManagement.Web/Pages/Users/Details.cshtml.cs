using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Contracts;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

public sealed class DetailsModel : PageModel
{
    private readonly IApplicationUserService _userService;
    public DetailsModel(IApplicationUserService userService) => _userService = userService;
    public UserListItemViewModel? UserItem { get; private set; }
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var dto = await _userService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();
        UserItem = new UserListItemViewModel { Id = dto.Id, Email = dto.Email, FirstName = dto.FirstName, LastName = dto.LastName, Gender = dto.Gender, DateOfBirth = dto.DateOfBirth, City = dto.City, State = dto.State };
        return Page();
    }
}
