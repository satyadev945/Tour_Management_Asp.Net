using FluentValidation;
using TourManagement.Application.DTOs;

namespace TourManagement.Application.Validators;

public sealed class BookingCreateDtoValidator : AbstractValidator<BookingCreateDto>
{
    public BookingCreateDtoValidator()
    {
        RuleFor(x => x.TourId).GreaterThan(0);
        RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(50);
    }
}

public sealed class BookingUpdateDtoValidator : AbstractValidator<BookingUpdateDto>
{
    public BookingUpdateDtoValidator()
    {
        Include(new BookingCreateDtoValidator());
    }
}
