namespace TourManagement.Application.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
}

public class BookingCreateDto
{
    public int TourId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class BookingUpdateDto : BookingCreateDto
{
}
