namespace Tour_Management.Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public Tour? Tour { get; set; }
}
