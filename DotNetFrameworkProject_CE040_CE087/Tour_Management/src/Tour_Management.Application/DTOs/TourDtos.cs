namespace Tour_Management.Application.DTOs;

public record TourDto(
    int Id,
    string Name,
    string Place,
    int Days,
    decimal Price,
    string Locations,
    string? Description,
    string? ImagePath);

public class TourCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public int Days { get; set; }
    public decimal Price { get; set; }
    public string Locations { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
}

public class TourUpdateDto : TourCreateDto
{
}
