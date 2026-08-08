using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

public sealed class UserListItemViewModel
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}

public sealed class UserRegistrationViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Gender { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; } = DateTime.UtcNow.Date;

    [Required, StringLength(200)]
    public string Street { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string State { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public sealed class UserEditViewModel
{
    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Gender { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required, StringLength(200)]
    public string Street { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string State { get; set; } = string.Empty;
}

public sealed class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
