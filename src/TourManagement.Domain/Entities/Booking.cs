namespace TourManagement.Domain.Entities;

/// <summary>
/// Represents a booking entity
/// </summary>
public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TourId { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public int NumberOfPeople { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled
    public string? SpecialRequests { get; set; }
    public DateTime? CancellationDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Tour Tour { get; set; } = null!;
}
