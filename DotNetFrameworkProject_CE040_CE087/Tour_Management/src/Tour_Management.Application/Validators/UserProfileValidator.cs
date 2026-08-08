using FluentValidation;
using Tour_Management.Application.DTOs;

namespace Tour_Management.Application.Validators;

public class UserProfileCreateDtoValidator : AbstractValidator<UserProfileCreateDto>
{
    public UserProfileCreateDtoValidator()
    {
        RuleFor(x => x.IdentityUserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).NotEmpty().MaximumLength(100);
    }
}

public class UserProfileUpdateDtoValidator : AbstractValidator<UserProfileUpdateDto>
{
    public UserProfileUpdateDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).NotEmpty().MaximumLength(100);
    }
}
