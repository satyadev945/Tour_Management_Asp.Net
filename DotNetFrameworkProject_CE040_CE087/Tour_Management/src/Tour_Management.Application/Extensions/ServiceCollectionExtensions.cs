using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tour_Management.Application.Mappings;
using Tour_Management.Application.Services;
using Tour_Management.Application.Validators;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IValidator<TourCreateDto>, TourCreateDtoValidator>();
        services.AddScoped<IValidator<TourUpdateDto>, TourUpdateDtoValidator>();
        services.AddScoped<IValidator<BookingCreateDto>, BookingCreateDtoValidator>();
        services.AddScoped<IValidator<UserProfileCreateDto>, UserProfileCreateDtoValidator>();
        services.AddScoped<IValidator<UserProfileUpdateDto>, UserProfileUpdateDtoValidator>();
        return services;
    }
}
