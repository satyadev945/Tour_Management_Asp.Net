using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TourManagement.Application.Contracts;
using TourManagement.Application.DTOs;
using TourManagement.Application.Mappings;
using TourManagement.Application.Services;
using TourManagement.Application.Validators;

namespace TourManagement.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton(provider => new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper());
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IApplicationUserService, ApplicationUserService>();
        services.AddScoped<IValidator<TourCreateDto>, TourCreateDtoValidator>();
        services.AddScoped<IValidator<TourUpdateDto>, TourUpdateDtoValidator>();
        services.AddScoped<IValidator<BookingCreateDto>, BookingCreateDtoValidator>();
        services.AddScoped<IValidator<BookingUpdateDto>, BookingUpdateDtoValidator>();
        services.AddScoped<IValidator<ApplicationUserCreateDto>, ApplicationUserCreateDtoValidator>();
        services.AddScoped<IValidator<ApplicationUserUpdateDto>, ApplicationUserUpdateDtoValidator>();
        return services;
    }
}
