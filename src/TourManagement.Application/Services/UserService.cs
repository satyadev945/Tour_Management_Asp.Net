using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Domain.Entities;
using TourManagement.Infrastructure.Interfaces;

namespace TourManagement.Application.Services;

/// <summary>
/// Service implementation for User operations
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving user with ID: {UserId}", id);
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID: {UserId}", id);
            throw;
        }
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        try
        {
            _logger.LogInformation("Retrieving user with email: {Email}", email);
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? MapToDto(user) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with email: {Email}", email);
            throw;
        }
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all users");
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            throw;
        }
    }

    public async Task<UserDto> RegisterUserAsync(RegisterUserDto registerDto)
    {
        try
        {
            _logger.LogInformation("Registering new user with email: {Email}", registerDto.Email);
            
            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("User with email {Email} already exists", registerDto.Email);
                throw new InvalidOperationException("User with this email already exists");
            }

            // Hash password (in production, use proper password hashing like BCrypt or ASP.NET Core Identity)
            var passwordHash = HashPassword(registerDto.Password);

            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                Role = "User"
            };

            var createdUser = await _userRepository.AddAsync(user);
            _logger.LogInformation("User registered successfully with ID: {UserId}", createdUser.Id);
            
            return MapToDto(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with email: {Email}", registerDto.Email);
            throw;
        }
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            _logger.LogInformation("User login attempt for email: {Email}", loginDto.Email);
            
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found with email: {Email}", loginDto.Email);
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            // Verify password (in production, use proper password verification)
            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed: Invalid password for email: {Email}", loginDto.Email);
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Login failed: User account is inactive for email: {Email}", loginDto.Email);
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "User account is inactive"
                };
            }

            _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);
            
            return new LoginResponseDto
            {
                Success = true,
                Message = "Login successful",
                User = MapToDto(user),
                Token = GenerateToken(user) // In production, use JWT tokens
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", loginDto.Email);
            throw;
        }
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UserDto userDto)
    {
        try
        {
            _logger.LogInformation("Updating user with ID: {UserId}", id);
            
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                _logger.LogWarning("User with ID: {UserId} not found", id);
                return null;
            }

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.PhoneNumber = userDto.PhoneNumber;
            existingUser.Address = userDto.Address;
            existingUser.IsActive = userDto.IsActive;
            existingUser.ModifiedDate = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            _logger.LogInformation("User updated successfully with ID: {UserId}", updatedUser.Id);
            
            return MapToDto(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID: {UserId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);
            var result = await _userRepository.DeleteAsync(id);
            
            if (result)
            {
                _logger.LogInformation("User deleted successfully with ID: {UserId}", id);
            }
            else
            {
                _logger.LogWarning("User with ID: {UserId} not found for deletion", id);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID: {UserId}", id);
            throw;
        }
    }

    public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
    {
        try
        {
            _logger.LogInformation("Changing password for user ID: {UserId}", userId);
            
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID: {UserId} not found", userId);
                return false;
            }

            if (!VerifyPassword(oldPassword, user.PasswordHash))
            {
                _logger.LogWarning("Password change failed: Invalid old password for user ID: {UserId}", userId);
                return false;
            }

            user.PasswordHash = HashPassword(newPassword);
            user.ModifiedDate = DateTime.UtcNow;
            
            await _userRepository.UpdateAsync(user);
            _logger.LogInformation("Password changed successfully for user ID: {UserId}", userId);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user ID: {UserId}", userId);
            throw;
        }
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            Role = user.Role,
            IsActive = user.IsActive
        };
    }

    // Simple password hashing (in production, use BCrypt, Argon2, or ASP.NET Core Identity)
    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string passwordHash)
    {
        var hashedInput = HashPassword(password);
        return hashedInput == passwordHash;
    }

    private static string GenerateToken(User user)
    {
        // In production, use JWT tokens with proper signing
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{user.Id}:{user.Email}:{DateTime.UtcNow.Ticks}"));
    }
}
