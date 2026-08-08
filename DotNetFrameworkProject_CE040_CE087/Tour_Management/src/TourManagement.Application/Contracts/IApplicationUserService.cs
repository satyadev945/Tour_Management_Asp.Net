using TourManagement.Application.DTOs;

namespace TourManagement.Application.Contracts;

public interface IApplicationUserService
{
    Task<IReadOnlyList<ApplicationUserDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApplicationUserDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ApplicationUserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<ApplicationUserDto> CreateAsync(ApplicationUserCreateDto dto, CancellationToken cancellationToken);
    Task UpdateAsync(int id, ApplicationUserUpdateDto dto, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ApplicationUserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken);
    Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken);
}
