using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Profile;

public class EditModel : PageModel
{
    private readonly IUserProfileService _profileService;
    private readonly UserManager<IdentityUser> _userManager;

    public EditModel(IUserProfileService profileService, UserManager<IdentityUser> userManager)
    {
        _profileService = profileService;
        _userManager = userManager;
    }

    [BindProperty]
    public ProfileInputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return RedirectToPage("/Account/Login");
        }

        var profile = await _profileService.GetByIdentityUserIdAsync(user.Id, cancellationToken);
        if (profile is not null)
        {
            Input = new ProfileInputModel
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender,
                DateOfBirth = profile.DateOfBirth,
                Street = profile.Street,
                City = profile.City,
                State = profile.State
            };
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return RedirectToPage("/Account/Login");
        }

        var existing = await _profileService.GetByIdentityUserIdAsync(user.Id, cancellationToken);
        if (existing is null)
        {
            await _profileService.CreateAsync(new UserProfileCreateDto
            {
                IdentityUserId = user.Id,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                DateOfBirth = Input.DateOfBirth,
                Street = Input.Street,
                City = Input.City,
                State = Input.State
            }, cancellationToken);
        }
        else
        {
            await _profileService.UpdateAsync(user.Id, new UserProfileUpdateDto
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                DateOfBirth = Input.DateOfBirth,
                Street = Input.Street,
                City = Input.City,
                State = Input.State
            }, cancellationToken);
        }

        return RedirectToPage("Index");
    }

    public class ProfileInputModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string Street { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string State { get; set; } = string.Empty;
    }
}
