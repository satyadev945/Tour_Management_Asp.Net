namespace Tour_Management.Application.DTOs;

public record BookingDto(
    int Id,
    int TourId,
    string TourName,
    string CustomerName,
    string Email,
    string Place,
    DateTime BookingDate);

public class BookingCreateDto
{
    public int TourId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
}
