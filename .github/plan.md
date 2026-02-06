# NuGet upgrade plan

Generated: 2026-02-06

## Summary

`dotnet list package --outdated` shows several packages are behind the latest available versions. The two test projects are already up to date, except for a patch update in `Audacia.Mail.Test.API`.

## Projects with upgrades available

### Audacia.Mail
- Audacia.CodeAnalysis: 1.5.1 -> 1.12.1

### Audacia.Mail.MailKit
- Audacia.CodeAnalysis: 1.5.1 -> 1.12.1
- MailKit: 2.5.2 -> 4.14.1

### Audacia.Mail.SendGrid
- Audacia.CodeAnalysis: 1.5.1 -> 1.12.1

### Audacia.Mail.MailTrap
- Audacia.CodeAnalysis: 1.5.1 -> 1.12.1

### Audacia.Mail.Local
- Audacia.CodeAnalysis: 1.5.1 -> 1.12.1

### Audacia.Mail.Log
- Audacia.CodeAnalysis: 1.5.1 -> 1.12.1

### Audacia.Mail.Noop
- Audacia.CodeAnalysis: 1.5.1 -> 1.12.1

### Audacia.Mail.Mandrill
- Audacia.CodeAnalysis: 1.5.1 -> 1.12.1
- Microsoft.Extensions.DependencyInjection.Abstractions: 6.0.0 -> 10.0.2
- Microsoft.Extensions.Http: 6.0.0 -> 10.0.2
- System.Net.Http.Json: 6.0.0 -> 10.0.2
- System.Text.Json: 9.0.0 -> 10.0.2

### Audacia.Mail.Test.API
- Swashbuckle.AspNetCore: 10.1.1 -> 10.1.2

## Projects with no updates

- Audacia.Mail.Test.App

## Notes

- All versions are based on the configured NuGet sources and the current target frameworks.
- This plan only lists available package updates; it does not apply upgrades.
