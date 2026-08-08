# Migration Notes

- Replaced ASP.NET Web Forms with ASP.NET Core Razor Pages.
- Replaced Web.config with appsettings.json.
- Replaced ADO.NET and SqlDataSource controls with EF Core repositories.
- Replaced insecure inline SQL login logic with hashed password validation.
- Introduced clean architecture with Domain, Application, Infrastructure, and Web layers.
