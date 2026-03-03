using TourManagement.Domain.DTOs;
/// <summary>

namespace TourManagement.Domain.Interfaces.Services;
/// Service interface for User business operations

/// </summary>
public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    
    Task<UserDto> CreateAsync(UserCreateDto createDto, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(int id, UserUpdateDto updateDto, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    
    Task<UserDto?> ValidateLoginAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default);
}
