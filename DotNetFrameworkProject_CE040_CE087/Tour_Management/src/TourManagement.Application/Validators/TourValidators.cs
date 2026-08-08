using FluentValidation;
using TourManagement.Application.DTOs;

namespace TourManagement.Application.Validators;

public sealed class TourCreateDtoValidator : AbstractValidator<TourCreateDto>
{
    public TourCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Place).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Days).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Locations).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}

public sealed class TourUpdateDtoValidator : AbstractValidator<TourUpdateDto>
{
    public TourUpdateDtoValidator()
    {
        Include(new TourCreateDtoValidator());
    }
}
