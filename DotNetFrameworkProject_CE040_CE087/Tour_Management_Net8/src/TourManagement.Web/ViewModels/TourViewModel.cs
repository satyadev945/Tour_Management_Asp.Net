using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

public class TourViewModel
{
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;
    
    [Required]
    public string Place { get; set; } = string.Empty;
    
    [Required]
    [Range(1, 365)]
    public int Days { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    
    [Required]
    public string Locations { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "Tour Information")]
    public string TourInfo { get; set; } = string.Empty;
    
    [Display(Name = "Picture")]
    public string? PictureFileName { get; set; }
    
    public IFormFile? PictureFile { get; set; }
}
