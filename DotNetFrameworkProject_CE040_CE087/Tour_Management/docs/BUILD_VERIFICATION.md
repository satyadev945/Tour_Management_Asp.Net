# Build Verification

Build verification should confirm:

- All projects target `net8.0`.
- EF Core 8 packages are used.
- No `System.Web`, `Web.config`, or `packages.config` dependencies remain in the active solution.
- Razor Pages application starts successfully.
