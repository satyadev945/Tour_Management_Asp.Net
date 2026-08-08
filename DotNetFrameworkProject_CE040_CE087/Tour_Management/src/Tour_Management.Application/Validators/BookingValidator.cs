using FluentValidation;
using Tour_Management.Application.DTOs;

namespace Tour_Management.Application.Validators;

public class BookingCreateDtoValidator : AbstractValidator<BookingCreateDto>
{
    public BookingCreateDtoValidator()
    {
        RuleFor(x => x.TourId).GreaterThan(0);
        RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Place).NotEmpty().MaximumLength(200);
    }
}
