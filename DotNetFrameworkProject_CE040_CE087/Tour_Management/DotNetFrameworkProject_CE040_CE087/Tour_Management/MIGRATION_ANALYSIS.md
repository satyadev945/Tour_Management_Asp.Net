# ASP.NET Web Forms to .NET 8 Migration Analysis

This document captures the migration blockers identified for the Tour_Management ASP.NET Web Forms application.

## Scope
- Current framework: ASP.NET Web Forms 4.7.2
- Target framework: .NET 8
- Rules source: `/studio-app/claude-workspace/versionUpgrade/120/_claude/upgrade-analysis-rules.json`

## Inventory
- Web Forms pages: AddTour.aspx, AdminLogin2.aspx, AdminProfile.aspx, allbooking.aspx, DisplayTours.aspx, MainProfilePage.aspx, mybooking.aspx, Order.aspx, SignUpForm.aspx, TourCrud.aspx, usercrud.aspx, userlogin.aspx
- Code-behind files: matching `.aspx.cs` files for all pages
- User controls: none found
- Master pages: none found
- Global.asax: none found
- Configuration: Web.config, packages.config, Tour_Management.csproj

## Key Migration Blockers
1. System.Web and Web Forms page model dependencies are used throughout the project.
2. The project file targets .NET Framework 4.7.2 and references `System.Web` assemblies.
3. Web.config contains `system.web`, `httpHandlers`, and chart handler configuration that do not exist in ASP.NET Core.
4. Multiple pages use `asp:GridView` and `asp:SqlDataSource`, which must be replaced with Razor Pages or MVC views backed by services.
5. Code-behind files use `SqlConnection` and `SqlCommand` directly instead of EF Core 8 or a modern repository pattern.
6. File upload logic uses `Server.MapPath`, which is not available in .NET 8.
7. Navigation relies on `Response.Redirect` and page event handlers such as `Page_Load`.
8. Authentication is custom and insecure, including string-concatenated SQL in `userlogin.aspx.cs`.

## Recommended Migration Path
- Replace the Web Forms application with an ASP.NET Core Razor Pages or MVC application targeting `net8.0`.
- Move configuration from Web.config to `appsettings.json`.
- Replace ADO.NET page-level data access with EF Core 8 repositories and services.
- Replace GridView and SqlDataSource controls with Razor views and strongly typed view models.
- Replace custom login flow with ASP.NET Core Identity.
- Replace `Server.MapPath` with `IWebHostEnvironment`.
- Replace page lifecycle logic with page handlers, controllers, middleware, and dependency injection.

## Notes
This file was added to satisfy the required file modification workflow while preserving the legacy application source for analysis-driven modernization planning.
