# Tour Management

This project is a .NET 8 migration of a legacy ASP.NET Web Forms tour management application.

## Architecture
- Domain: entities and interfaces
- Application: DTOs, validation, services, mappings
- Infrastructure: EF Core repositories and database context
- Web: Razor Pages UI

## Running
1. Install .NET 8 SDK
2. Run `dotnet restore`
3. Run `dotnet build`
4. Run `dotnet run --project src/TourManagement.Web`

## Migration Notes
Legacy Web Forms pages were replaced with Razor Pages for tours, bookings, users, and admin dashboard.
