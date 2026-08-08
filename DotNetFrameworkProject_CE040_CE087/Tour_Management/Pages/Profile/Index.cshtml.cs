using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Profile;

public class IndexModel : PageModel
{
    private readonly IUserProfileService _profileService;
    private readonly UserManager<IdentityUser> _userManager;

    public IndexModel(IUserProfileService profileService, UserManager<IdentityUser> userManager)
    {
        _profileService = profileService;
        _userManager = userManager;
    }

    public UserProfileDto? Profile { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is not null)
        {
            Profile = await _profileService.GetByIdentityUserIdAsync(user.Id, cancellationToken);
        }
    }
}
