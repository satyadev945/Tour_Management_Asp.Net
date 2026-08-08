using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

public sealed class BookingListItemViewModel
{
    public int Id { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
}

public sealed class BookingFormViewModel
{
    [Range(1, int.MaxValue)]
    public int TourId { get; set; }

    [Required, StringLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string PhoneNumber { get; set; } = string.Empty;
}
