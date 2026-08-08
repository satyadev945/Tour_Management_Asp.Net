# Tour Management

This project is a .NET 8 Razor Pages migration of the legacy ASP.NET Web Forms Tour Management application.

## Architecture
- `src/Tour_Management.Domain`: entities and interfaces
- `src/Tour_Management.Application`: DTOs, services, validation, mappings
- `src/Tour_Management.Infrastructure`: EF Core data access and seeding
- Root web project: Razor Pages UI and ASP.NET Core Identity

## Running
1. Ensure .NET 8 SDK is installed.
2. Run `dotnet restore Tour_Management.sln` (or `dotnet restore Tour_Management.csproj`).
3. Run `dotnet build Tour_Management.sln` (or `dotnet build Tour_Management.csproj`).
4. Run `dotnet run --project Tour_Management.csproj`.

> Important: if you run `dotnet build` or `dotnet restore` from another directory, explicitly pass the solution or project path. This avoids `MSB1003: Specify a project or solution file`.

## Default admin
- Email: `admin@tourmanagement.local`
- Password: `Admin123!`
