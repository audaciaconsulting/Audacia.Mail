## Audacia.Mail
Standardized interfaces for common email-sending functionality. The library with this name contains all of the interfaces and types used by the implementations.

### Usage
This set of libraries facilitates the sending of emails through a standard set of interfaces. Several different implementations are provided:

#### `Audacia.Mail.Local`
Sends email to the local machine to be captured by a locally hosted SMTP server such as [Papercut](https://github.com/changemakerstudios/papercut) or [smtp4dev](https://github.com/rnwood/smtp4dev).
This library can automatically install (if needed) and start the specified SMTP server when debugging. Papercut is recommended as it doesn't require any configuration whereas smtp4dev needs authentication disabled in order to work.

#### `Audacia.Mail.MailKit`
This library uses standard SMTP protocol to send mails, implemented with MailKit.

#### `Audacia.Mail.Mailtrap`
Send mail to the Mailtrap server for testing purposes. Uses the MailKit SMTP implementation.

#### `Audacia.Mail.SendGrid`
Send mail using the SendGrid API.