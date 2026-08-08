using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Contracts;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

public sealed class IndexModel : PageModel
{
    private readonly IApplicationUserService _userService;
    public IndexModel(IApplicationUserService userService) => _userService = userService;
    public IReadOnlyList<UserListItemViewModel> Users { get; private set; } = [];
    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllAsync(cancellationToken);
        Users = users.Select(x => new UserListItemViewModel { Id = x.Id, Email = x.Email, FirstName = x.FirstName, LastName = x.LastName, Gender = x.Gender, DateOfBirth = x.DateOfBirth, City = x.City, State = x.State }).ToList();
    }
}
