using FluentValidation;
using TourManagement.Application.DTOs;

namespace TourManagement.Application.Validators;

public sealed class ApplicationUserCreateDtoValidator : AbstractValidator<ApplicationUserCreateDto>
{
    public ApplicationUserCreateDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Gender).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(200);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}

public sealed class ApplicationUserUpdateDtoValidator : AbstractValidator<ApplicationUserUpdateDto>
{
    public ApplicationUserUpdateDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Gender).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(200);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).NotEmpty().MaximumLength(100);
    }
}
