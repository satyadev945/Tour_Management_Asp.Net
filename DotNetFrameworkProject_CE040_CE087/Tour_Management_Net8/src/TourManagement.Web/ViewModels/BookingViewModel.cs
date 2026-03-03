using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

public class BookingViewModel
{
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "User")]
    public int UserId { get; set; }
    
    [Required]
    [Display(Name = "Tour")]
    public int TourId { get; set; }
    
    [Required]
    [Display(Name = "Booking Date")]
    [DataType(DataType.Date)]
    public DateTime BookingDate { get; set; }
    
    [Required]
    [Range(1, 100)]
    [Display(Name = "Number of People")]
    public int NumberOfPeople { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    [DataType(DataType.Currency)]
    [Display(Name = "Total Amount")]
    public decimal TotalAmount { get; set; }
    
    public string Status { get; set; } = "Pending";
    
    public string? UserEmail { get; set; }
    
    public string? TourName { get; set; }
}
