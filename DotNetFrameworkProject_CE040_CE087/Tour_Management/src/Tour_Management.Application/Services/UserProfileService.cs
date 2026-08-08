using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Repositories;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserProfileService> _logger;
    private readonly IValidator<UserProfileCreateDto> _createValidator;
    private readonly IValidator<UserProfileUpdateDto> _updateValidator;

    public UserProfileService(
        IUserProfileRepository repository,
        IMapper mapper,
        ILogger<UserProfileService> logger,
        IValidator<UserProfileCreateDto> createValidator,
        IValidator<UserProfileUpdateDto> updateValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<UserProfileDto?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken)
    {
        var profile = await _repository.GetByIdentityUserIdAsync(identityUserId, cancellationToken);
        return profile is null ? null : _mapper.Map<UserProfileDto>(profile);
    }

    public async Task<UserProfileDto> CreateAsync(UserProfileCreateDto dto, CancellationToken cancellationToken)
    {
        await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);
        var entity = _mapper.Map<ApplicationUserProfile>(dto);
        var created = await _repository.AddAsync(entity, cancellationToken);
        _logger.LogInformation("Created profile for user {UserId}", dto.IdentityUserId);
        return _mapper.Map<UserProfileDto>(created);
    }

    public async Task UpdateAsync(string identityUserId, UserProfileUpdateDto dto, CancellationToken cancellationToken)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);
        var existing = await _repository.GetByIdentityUserIdAsync(identityUserId, cancellationToken) ?? throw new InvalidOperationException("Profile not found.");
        _mapper.Map(dto, existing);
        await _repository.UpdateAsync(existing, cancellationToken);
        _logger.LogInformation("Updated profile for user {UserId}", identityUserId);
    }
}
