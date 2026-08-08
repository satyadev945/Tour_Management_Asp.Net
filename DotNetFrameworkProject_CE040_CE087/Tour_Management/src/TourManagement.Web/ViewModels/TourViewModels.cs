using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

public sealed class TourListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public int Days { get; set; }
    public decimal Price { get; set; }
    public string Locations { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
}

public sealed class TourFormViewModel
{
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Place { get; set; } = string.Empty;

    [Range(1, 365)]
    public int Days { get; set; }

    [Range(typeof(decimal), "0.01", "999999")]
    public decimal Price { get; set; }

    [Required, StringLength(500)]
    public string Locations { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Display(Name = "Image path")]
    public string? ImagePath { get; set; }
}
