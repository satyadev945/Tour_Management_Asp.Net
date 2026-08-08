# Tour_Management Migration Notes

## Purpose
This module remains an ASP.NET Web Forms application. A direct framework retarget to .NET 8 is not feasible because Web Forms and `System.Web` are unsupported on .NET 8.

## Immediate blockers
- `System.Web` references in project and code-behind files
- Web Forms pages (`.aspx`) and designer files
- `GridView` and `SqlDataSource` controls
- `Web.config` runtime configuration
- Non-SDK-style `.csproj`
- Legacy package `Microsoft.CodeDom.Providers.DotNetCompilerPlatform`

## Suggested target architecture
- ASP.NET Core Razor Pages or MVC for UI
- EF Core 8 for data access
- ASP.NET Core Identity for authentication
- `appsettings.json` for configuration
- Dependency injection for services and repositories

## Important examples from current code
- `AddTour.aspx.cs` uses `Server.MapPath` and `FileUpload.SaveAs`
- `userlogin.aspx.cs` builds SQL with string concatenation
- `TourCrud.aspx`, `usercrud.aspx`, `allbooking.aspx`, `mybooking.aspx`, and `DisplayTours.aspx` use `SqlDataSource` and `GridView`

## Outcome
Use the generated migration analysis report from the assistant response together with `MIGRATION_ANALYSIS.md` as the basis for a full ASP.NET Core rewrite.
