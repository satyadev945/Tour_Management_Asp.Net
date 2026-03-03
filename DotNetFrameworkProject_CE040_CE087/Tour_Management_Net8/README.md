# Tour Management System - .NET 8 Migration

## Overview
This is a modern .NET 8 web application migrated from ASP.NET Web Forms 4.7.2. The application follows clean architecture principles with a layered structure.

## Architecture

### Project Structure
```
Tour_Management_Net8/
├── src/
│   ├── TourManagement.Domain/          # Domain entities and interfaces
│   ├── TourManagement.Application/     # Business logic and services
│   ├── TourManagement.Infrastructure/  # Data access and repositories
│   └── TourManagement.Web/             # Razor Pages UI
├── tests/
│   ├── TourManagement.UnitTests/       # Unit tests
│   └── TourManagement.IntegrationTests/# Integration tests
└── docs/                               # Documentation
```

### Technology Stack
- **.NET 8.0** - Target framework
- **ASP.NET Core Razor Pages** - Web UI
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Database
- **AutoMapper 12.0** - Object mapping
- **Serilog 8.0** - Logging
- **xUnit** - Testing framework

## Features
- Tour management (CRUD operations)
- User registration and authentication
- Booking management
- Search functionality
- File upload for tour images

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Setup Instructions

1. **Clone the repository**
   ```bash
   cd /path/to/Tour_Management_Net8
   ```

2. **Update connection string**
   Edit `src/TourManagement.Web/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Your-Connection-String-Here"
   }
   ```

3. **Create database**
   ```bash
   cd src/TourManagement.Infrastructure
   dotnet ef migrations add InitialCreate --startup-project ../TourManagement.Web
   dotnet ef database update --startup-project ../TourManagement.Web
   ```

4. **Build the solution**
   ```bash
   dotnet build
   ```

5. **Run the application**
   ```bash
   cd src/TourManagement.Web
   dotnet run
   ```

6. **Access the application**
   Open browser to: https://localhost:5001

## Migration Notes

### Key Changes from Web Forms
- **UI Framework**: Migrated from Web Forms (.aspx) to Razor Pages (.cshtml)
- **Data Access**: Replaced ADO.NET with Entity Framework Core
- **Configuration**: Moved from Web.config to appsettings.json
- **Dependency Injection**: Using built-in ASP.NET Core DI
- **Logging**: Replaced with Serilog
- **State Management**: Session state configured for distributed cache support

### Breaking Changes
- ViewState is no longer available - use TempData or hidden fields
- Server controls replaced with HTML helpers and Tag Helpers
- Global.asax logic moved to Program.cs
- HttpContext.Current replaced with IHttpContextAccessor

## Testing

### Run Unit Tests
```bash
cd tests/TourManagement.UnitTests
dotnet test
```

### Run Integration Tests
```bash
cd tests/TourManagement.IntegrationTests
dotnet test
```

## Project Dependencies

### Domain Layer
- No external dependencies (pure domain logic)

### Application Layer
- AutoMapper 12.0.1
- FluentValidation 11.9.0
- Microsoft.Extensions.Logging.Abstractions 8.0.0

### Infrastructure Layer
- Microsoft.EntityFrameworkCore 8.0.0
- Microsoft.EntityFrameworkCore.SqlServer 8.0.0
- Microsoft.EntityFrameworkCore.Design 8.0.0
- Dapper 2.1.28

### Web Layer
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 8.0.0
- Serilog.AspNetCore 8.0.0
- Serilog.Sinks.Console 5.0.0
- Serilog.Sinks.File 5.0.0

## Configuration

### Database Connection
Configure in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TourManagementDb;Trusted_Connection=true"
  }
}
```

### Logging
Serilog is configured to write to:
- Console (for development)
- File (logs/tourmanagement-{Date}.txt)

## Known Issues
- None at this time

## Future Enhancements
- Implement proper authentication with ASP.NET Core Identity
- Add authorization policies
- Implement caching for frequently accessed data
- Add API endpoints for mobile app integration
- Implement real-time notifications with SignalR

## Support
For issues or questions, please contact the development team.

## License
Copyright © 2024 Tour Management System
