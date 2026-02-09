# Changelog

## 1.3.6 - 2026-02-09
### Added
- No new functionality added

### Changed
- Upgraded test apps to target net10.0.
- Updated dependencies including Audacia.CodeAnalysis, MailKit, Swashbuckle, and related Microsoft.Extensions packages.

### Fixed
- Dispose MailKit MimeMessage after sending to avoid resource leaks.
- Throw a clear error when required SmtpOptions configuration is missing.

## 1.3.5 - 2024-11-21
### Added 
- No new functionality added

### Changed
- No functionality changed

### Fixed
- Updated vulnerable dependencies ([#7](https://github.com/audaciaconsulting/Audacia.Mail/pull/7))

## 1.3.4 - 2024-09-19
### Added
- Bcc functionality added for SendGrid client.

## 1.3.1 - 2024-07-03
### Added
- No new functionality added

### Changed
- Added support for passing in the host to the MailTrapClient ([20c88b7](https://github.com/audaciaconsulting/Audacia.Mail/pull/4/commits/20c88b7d4563a76b102e081fc988b18880419f94))

## 1.3.0 - 2024-04-25
### Added
- No new functionality added

### Changed
- Upgraded SendGrid to version 9.29.3 ([c6d6a11](https://github.com/audaciaconsulting/Audacia.Mail/pull/2/commits/c6d6a11107c5354486d65b99fe102096cffe1c07))
