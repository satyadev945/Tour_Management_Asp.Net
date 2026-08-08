namespace Tour_Management.Domain.Interfaces.Services;

public interface IUserProfileService
{
    Task<Tour_Management.Application.DTOs.UserProfileDto?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken);
    Task<Tour_Management.Application.DTOs.UserProfileDto> CreateAsync(Tour_Management.Application.DTOs.UserProfileCreateDto dto, CancellationToken cancellationToken);
    Task UpdateAsync(string identityUserId, Tour_Management.Application.DTOs.UserProfileUpdateDto dto, CancellationToken cancellationToken);
}
