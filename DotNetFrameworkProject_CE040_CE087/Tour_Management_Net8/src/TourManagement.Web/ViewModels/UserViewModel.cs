using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Display(Name = "First Name")]
    public string? FirstName { get; set; }
    
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }
    
    [Phone]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }
}

public class UserRegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    [Display(Name = "First Name")]
    public string? FirstName { get; set; }
    
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }
    
    [Phone]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }
}

public class UserLoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
