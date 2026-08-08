using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Application.Contracts;

namespace TourManagement.Application.Services;

public sealed class ApplicationUserService : IApplicationUserService
{
    private readonly IApplicationUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ApplicationUserService> _logger;
    private readonly IValidator<ApplicationUserCreateDto> _createValidator;
    private readonly IValidator<ApplicationUserUpdateDto> _updateValidator;

    public ApplicationUserService(
        IApplicationUserRepository repository,
        IMapper mapper,
        ILogger<ApplicationUserService> logger,
        IValidator<ApplicationUserCreateDto> createValidator,
        IValidator<ApplicationUserUpdateDto> updateValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IReadOnlyList<ApplicationUserDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ApplicationUserDto>>(entities);
    }

    public async Task<ApplicationUserDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : _mapper.Map<ApplicationUserDto>(entity);
    }

    public async Task<ApplicationUserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByEmailAsync(email, cancellationToken);
        return entity is null ? null : _mapper.Map<ApplicationUserDto>(entity);
    }

    public async Task<ApplicationUserDto> CreateAsync(ApplicationUserCreateDto dto, CancellationToken cancellationToken)
    {
        await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);
        var existing = await _repository.GetByEmailAsync(dto.Email, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException("A user with the same email already exists.");
        }

        var entity = new ApplicationUser
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            Street = dto.Street,
            City = dto.City,
            State = dto.State,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        var created = await _repository.AddAsync(entity, cancellationToken);
        _logger.LogInformation("Created user {Email}", created.Email);
        return _mapper.Map<ApplicationUserDto>(created);
    }

    public async Task UpdateAsync(int id, ApplicationUserUpdateDto dto, CancellationToken cancellationToken)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);
        var existing = await _repository.GetByIdAsync(id, cancellationToken) ?? throw new EntityNotFoundException($"User {id} was not found.");
        existing.FirstName = dto.FirstName;
        existing.LastName = dto.LastName;
        existing.Gender = dto.Gender;
        existing.DateOfBirth = dto.DateOfBirth;
        existing.Street = dto.Street;
        existing.City = dto.City;
        existing.State = dto.State;
        existing.ModifiedDate = DateTime.UtcNow;
        await _repository.UpdateAsync(existing, cancellationToken);
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken) => _repository.DeleteAsync(id, cancellationToken);

    public async Task<IReadOnlyList<ApplicationUserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken)
    {
        var entities = await _repository.SearchAsync(searchTerm, cancellationToken);
        return _mapper.Map<IReadOnlyList<ApplicationUserDto>>(entities);
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmailAsync(email, cancellationToken);
        return user is not null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}
