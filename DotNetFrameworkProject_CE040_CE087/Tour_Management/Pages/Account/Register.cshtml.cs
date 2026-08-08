using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserProfileService _profileService;

    public RegisterModel(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IUserProfileService profileService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _profileService = profileService;
    }

    [BindProperty]
    public RegisterInputModel Input { get; set; } = new();

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new IdentityUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(user, Input.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

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

        await _signInManager.SignInAsync(user, true);
        return RedirectToPage("/Index");
    }

    public class RegisterInputModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
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
