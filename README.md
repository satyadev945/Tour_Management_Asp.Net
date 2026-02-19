# Tour Management System - .NET 8

A modern tour management system built with ASP.NET Core 8.0 MVC, following clean architecture principles.

## Architecture

This application follows Clean Architecture with the following layers:

- **Domain Layer** (`TourManagement.Domain`): Contains entities and domain logic
- **Application Layer** (`TourManagement.Application`): Contains business logic, services, and DTOs
- **Infrastructure Layer** (`TourManagement.Infrastructure`): Contains data access, repositories, and EF Core DbContext
- **Web Layer** (`TourManagement.Web`): ASP.NET Core MVC application with controllers and views

## Technologies

- .NET 8.0
- ASP.NET Core MVC
- Entity Framework Core 8.0
- SQL Server (LocalDB)
- xUnit for testing
- Moq for mocking

## Features

- Tour management (CRUD operations)
- User registration and authentication
- Booking system
- Admin dashboard
- Session-based authentication
- File upload for tour images

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server LocalDB or SQL Server

### Setup

1. Clone the repository
2. Update the connection string in `src/TourManagement.Web/appsettings.json`
3. Run database migrations:
   ```bash
   cd src/TourManagement.Web
   dotnet ef database update
   ```
4. Build the solution:
   ```bash
   dotnet build
   ```
5. Run the application:
   ```bash
   cd src/TourManagement.Web
   dotnet run
   ```

## Project Structure

```
TourManagement/
├── src/
│   ├── TourManagement.Domain/          # Domain entities
│   ├── TourManagement.Application/     # Business logic and services
│   ├── TourManagement.Infrastructure/  # Data access and repositories
│   └── TourManagement.Web/            # ASP.NET Core MVC application
├── tests/
│   └── TourManagement.Tests/          # Unit tests
└── TourManagement.sln                 # Solution file
```

## Database Schema

### Tables

- **Tour**: Tour packages with details (name, place, days, price, locations, info, picture)
- **Userinfo**: User accounts with authentication
- **Booking**: Tour bookings with user and tour relationships

## API Endpoints

### Tours
- GET `/Tour/Index` - List all tours (Admin)
- GET `/Tour/Display` - Display active tours
- GET `/Tour/Details/{id}` - View tour details
- GET `/Tour/Create` - Create tour form (Admin)
- POST `/Tour/Create` - Create new tour (Admin)
- GET `/Tour/Edit/{id}` - Edit tour form (Admin)
- POST `/Tour/Edit/{id}` - Update tour (Admin)
- GET `/Tour/Delete/{id}` - Delete confirmation (Admin)
- POST `/Tour/Delete/{id}` - Delete tour (Admin)

### Users
- GET `/User/Login` - User login page
- POST `/User/Login` - User login
- GET `/User/Register` - User registration page
- POST `/User/Register` - Register new user
- GET `/User/Profile` - User profile
- GET `/User/Logout` - Logout

### Bookings
- GET `/Booking/Index` - List bookings
- GET `/Booking/MyBookings` - User's bookings
- GET `/Booking/Create` - Create booking form
- POST `/Booking/Create` - Create new booking
- POST `/Booking/Cancel/{id}` - Cancel booking

### Admin
- GET `/Admin/Login` - Admin login page
- POST `/Admin/Login` - Admin login
- GET `/Admin/Profile` - Admin profile
- GET `/Admin/Dashboard` - Admin dashboard

## Testing

Run tests using:
```bash
dotnet test
```

## Migration from ASP.NET Web Forms

This application was migrated from ASP.NET Web Forms 4.7.2 to .NET 8.0. Key changes include:

1. **Architecture**: Moved from monolithic Web Forms to Clean Architecture
2. **UI**: Migrated from .aspx pages to Razor views with MVC pattern
3. **Data Access**: Replaced ADO.NET with Entity Framework Core
4. **Authentication**: Moved from Forms Authentication to session-based authentication
5. **Dependency Injection**: Implemented built-in DI container
6. **Configuration**: Migrated from Web.config to appsettings.json
7. **Async/Await**: Implemented async patterns throughout

## Security Notes

- Passwords are hashed using SHA256 (for production, use BCrypt or ASP.NET Core Identity)
- Session-based authentication is used (consider JWT tokens for production)
- HTTPS is enforced
- Anti-forgery tokens are used on forms

## Future Enhancements

- Implement ASP.NET Core Identity for authentication
- Add JWT token-based authentication
- Implement email notifications
- Add payment gateway integration
- Implement caching
- Add API endpoints for mobile apps
- Implement real-time notifications with SignalR

## License

This project is licensed under the MIT License.
