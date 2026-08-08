namespace TourManagement.Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = "system";
    public string? ModifiedBy { get; set; }

    public Tour? Tour { get; set; }
}
