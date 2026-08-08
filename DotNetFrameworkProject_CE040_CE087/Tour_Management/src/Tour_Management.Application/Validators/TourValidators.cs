using FluentValidation;
using Tour_Management.Application.DTOs;

namespace Tour_Management.Application.Validators;

public class TourCreateDtoValidator : AbstractValidator<TourCreateDto>
{
    public TourCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Place).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Days).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Locations).NotEmpty().MaximumLength(500);
    }
}

public class TourUpdateDtoValidator : AbstractValidator<TourUpdateDto>
{
    public TourUpdateDtoValidator()
    {
        Include(new TourCreateDtoValidator());
    }
}
