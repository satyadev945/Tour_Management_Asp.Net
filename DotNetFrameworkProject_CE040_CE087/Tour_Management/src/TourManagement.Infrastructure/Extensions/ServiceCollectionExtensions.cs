using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Infrastructure.Data;
using TourManagement.Infrastructure.Repositories;

namespace TourManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=tourmanagement.db";
        services.AddDbContext<TourManagementDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<ITourRepository, TourRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        return services;
    }
}
