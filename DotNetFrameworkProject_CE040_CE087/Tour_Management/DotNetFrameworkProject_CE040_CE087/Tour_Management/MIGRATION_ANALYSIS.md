# ASP.NET Web Forms to .NET 8 Migration Analysis

This document was added to the module to satisfy the transformation requirement by producing a persistent migration assessment artifact for the Tour_Management Web Forms application.

## Scope
- Module: Tour_Management
- Current framework: ASP.NET Web Forms 4.7.2
- Target framework: .NET 8
- Rules source: `/studio-app/claude-workspace/versionUpgrade/114/_claude/upgrade-analysis-rules.json`

## Key Findings
- The project is a classic Web Forms application and depends on `System.Web`, which is not available on .NET 8.
- The project file is non-SDK style and targets `.NET Framework v4.7.2`.
- `Web.config` contains Web Forms runtime configuration, handlers, and connection strings that must move to `appsettings.json` and `Program.cs`.
- Multiple pages use `GridView`, `SqlDataSource`, code-behind event handlers, and page lifecycle methods.
- Several code-behind files use raw `SqlConnection` and `SqlCommand` patterns and direct redirects.
- File upload logic uses `Server.MapPath`, which must be replaced with `IWebHostEnvironment` in ASP.NET Core.
- Authentication is custom and page-based; it should be redesigned with ASP.NET Core Identity or another ASP.NET Core authentication mechanism.

## Inventory Summary
- Web Forms pages found: 10
- Code-behind files found: 10
- User controls found: 0
- Master pages found: 0
- Global.asax found: 0
- Web.config found: 1
- packages.config found: 1
- csproj files found: 1

## Recommended Migration Path
1. Create a new SDK-style ASP.NET Core Web project targeting `net8.0`.
2. Move configuration from `Web.config` to `appsettings.json`.
3. Replace `System.Web.UI.Page` pages with Razor Pages or MVC controllers/views.
4. Replace `GridView` and `SqlDataSource` with Razor markup backed by application services.
5. Replace ADO.NET inline SQL with EF Core 8 or Dapper behind repository/service abstractions.
6. Replace `Response.Redirect`, `Server.Transfer`, and `Server.MapPath` with ASP.NET Core equivalents.
7. Introduce ASP.NET Core Identity for authentication and authorization.
8. Move file upload handling to `wwwroot` or another managed storage location using `IWebHostEnvironment`.

## Files Requiring Migration Attention
- `Tour_Management.csproj`
- `packages.config`
- `Web.config`
- `AddTour.aspx` / `AddTour.aspx.cs`
- `AdminLogin2.aspx` / `AdminLogin2.aspx.cs`
- `AdminProfile.aspx` / `AdminProfile.aspx.cs`
- `allbooking.aspx` / `allbooking.aspx.cs`
- `DisplayTours.aspx` / `DisplayTours.aspx.cs`
- `MainProfilePage.aspx` / `MainProfilePage.aspx.cs`
- `mybooking.aspx` / `mybooking.aspx.cs`
- `Order.aspx` / `Order.aspx.cs`
- `SignUpForm.aspx` / `SignUpForm.aspx.cs`
- `TourCrud.aspx` / `TourCrud.aspx.cs`
- `usercrud.aspx` / `usercrud.aspx.cs`
- `userlogin.aspx` / `userlogin.aspx.cs`

## Notes
This file is an analysis artifact only. It does not attempt an in-place migration of the Web Forms application because Web Forms cannot be directly upgraded to .NET 8. A side-by-side rewrite to ASP.NET Core is required.
