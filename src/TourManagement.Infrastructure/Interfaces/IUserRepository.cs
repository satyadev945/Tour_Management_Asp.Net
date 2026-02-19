using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Interfaces;

/// <summary>
/// Repository interface for User entity
/// </summary>
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
