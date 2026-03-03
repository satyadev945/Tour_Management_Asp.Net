namespace TourManagement.Domain.DTOs;
/// </summary>
public class BookingDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TourId { get; set; }
    public DateTime BookingDate { get; set; }
    public int NumberOfPeople { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    
    // Related entity information
    public string? UserEmail { get; set; }
    public string? TourName { get; set; }
}

/// <summary>
/// DTO for creating a new booking
/// </summary>
public class BookingCreateDto
{
    public int UserId { get; set; }
    public int TourId { get; set; }
    public DateTime BookingDate { get; set; }
    public int NumberOfPeople { get; set; }
    public decimal TotalAmount { get; set; }
}

/// <summary>
/// DTO for updating an existing booking
/// </summary>
public class BookingUpdateDto
{
    public DateTime BookingDate { get; set; }
    public int NumberOfPeople { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
}
