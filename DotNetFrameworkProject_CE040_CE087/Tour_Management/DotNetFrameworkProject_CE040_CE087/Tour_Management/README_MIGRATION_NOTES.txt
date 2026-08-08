Tour_Management migration notes

This legacy module is an ASP.NET Web Forms application and cannot be upgraded in place to .NET 8.

Primary blockers identified:
- System.Web dependencies
- Web Forms pages and code-behind model
- Web.config and legacy handlers
- GridView and SqlDataSource controls
- Direct ADO.NET usage in page event handlers
- Custom login flow without ASP.NET Core Identity

Recommended target architecture:
- ASP.NET Core Web application targeting net8.0
- appsettings.json for configuration
- EF Core 8 for data access
- Razor Pages or MVC for UI
- ASP.NET Core Identity for authentication

See MIGRATION_ANALYSIS.md for the detailed inventory and blocker summary.
