# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade Audacia.Mail.Test.API\Audacia.Mail.Test.API.csproj
4. Upgrade Audacia.Mail.Test.App\Audacia.Mail.Test.App.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

The following projects are excluded from the upgrade as they are not test applications:

| Project name                                           | Description                                  |
|:-------------------------------------------------------|:--------------------------------------------:|
| Audacia.Mail\Audacia.Mail.csproj                       | Not a test project                           |
| Audacia.Mail.MailKit\Audacia.Mail.MailKit.csproj       | Not a test project                           |
| Audacia.Mail.Local\Audacia.Mail.Local.csproj           | Not a test project                           |
| Audacia.Mail.SendGrid\Audacia.Mail.SendGrid.csproj     | Not a test project                           |
| Audacia.Mail.MailTrap\Audacia.Mail.MailTrap.csproj     | Not a test project                           |
| Audacia.Mail.Log\Audacia.Mail.Log.csproj               | Not a test project                           |
| Audacia.Mail.Noop\Audacia.Mail.Noop.csproj             | Not a test project                           |
| Audacia.Mail.Mandrill\Audacia.Mail.Mandrill.csproj     | Not a test project                           |

### Aggregate NuGet packages modifications across all projects

NuGet packages used across test projects that will be upgraded to their latest compatible versions for .NET 10.0.

| Package Name                                           | Current Version | New Version | Description                                   |
|:-------------------------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Audacia.CodeAnalysis                                   | 1.5.1           | Latest      | Upgrade to latest compatible version          |
| Audacia.Random                                         | 3.0.55181.19352 | Latest      | Upgrade to latest compatible version          |

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### Audacia.Mail.Test.API\Audacia.Mail.Test.API.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net10.0`

NuGet packages changes:
  - All NuGet dependencies upgraded to their latest compatible versions for .NET 10.0

#### Audacia.Mail.Test.App\Audacia.Mail.Test.App.csproj modifications

Project properties changes:
  - Target framework should be changed from `net6.0` to `net10.0`

NuGet packages changes:
  - Audacia.CodeAnalysis upgraded to latest compatible version
  - Audacia.Random upgraded to latest compatible version
