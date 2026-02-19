using TourManagement.Application.DTOs;

namespace TourManagement.Application.Interfaces;

/// <summary>
/// Service interface for User operations
/// </summary>
public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> RegisterUserAsync(RegisterUserDto registerDto);
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<UserDto?> UpdateUserAsync(int id, UserDto userDto);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
}
