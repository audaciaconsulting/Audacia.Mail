# Audacia.Mail
Standardized interfaces for common email-sending functionality. The library with this name contains all of the interfaces and types used by the implementations.

## Contents
1. [Usage](#usage)
1. [Implementations](#implementations)

## Usage
This set of libraries facilitates the sending of emails through a standard set of interfaces each implementing the IMailClient interface. Several different implementations are provided:

### Configuring The Email Library
Before you can start using the email libraries functionality you need to first set up the configuration.

You can add the emailing to your service collection by using the `appsettings.json` found in your api project.
By using the `IConfiguration` interface you can get the SmtpOptions from the `appsettings.json` as seen in the example below.
```csharp
public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
{
    if (configuration == null)
    {
        throw new ArgumentNullException(nameof(configuration));
    }

    var smtpOptions = configuration.GetSection("SmtpOptions").Get<SmtpOptions>();
    var senderAddress = smtpOptions.FromEmailAddress != null
        ? new EmailSenderDetails(smtpOptions.FromEmailAddress)
        : new EmailSenderDetails();

    return services
        .AddSingleton(smtpOptions)
        .AddSingleton(senderAddress)
        .AddMailClient(smtpOptions);
}
```

As you can see below you can pass through empty/`null` strings for the `UserName` and `Password` when using `SmtpSettings` if the SMTP server is unauthenticated.
```json
  "SmtpOptions": {
    "EmailClientType": "Local",
    "FromEmailAddress": null,
    "UserName": null,
    "Password": null,
    "Host": null,
    "Port": 25,
    "EnableSsl": false
  }
```

The above example uses the `SmtpOptions` class to store the settings found in the `appsettings.json` to public variables.

`SmtpSettings` is a class within `Audacia.Mail` used to store the core STMP-related properties. `SmtpOptions` is a class that extends `SmtpSettings` with some more specific properties.
```csharp
public class SmtpOptions : SmtpSettings
{
    public EmailClientType EmailClientType { get; set; }

    public string? FromEmailAddress { get; set; }

    public bool EnableSsl { get; set; }
}
```

Whilst the `EmailSenderDetails` class keeps hold of the fromAddress from the `appsettings.json`.
```csharp
public class EmailSenderDetails
{
    public EmailSenderDetails() =>
        Address = new MailAddress();

    public EmailSenderDetails(string fromAddress) =>
        Address = new MailAddress(fromAddress);

    public MailAddress Address { get; }
}
```

### Setting Up Email Client.
To start using the Audacia mail library you need to set up the email client in your project. As seen in the above example `AddEmailServices` the configuration references `AddEmailServices`. This method creates a client factory, returning a different `IMailClient` based on whichever enum is found in the `appsettings.json` file.
```csharp
private static IServiceCollection AddMailClient(this IServiceCollection services, SmtpOptions smtpOptions)
{
    Func<IServiceProvider, IMailClient> mailClientFactory = smtpOptions.EmailClientType switch
    {
        EmailClientType.None => _ => new NoopMailClient(),
        EmailClientType.Local => _ => new DevMailClient(ServerType.Papercut, smtpOptions.Port),
        EmailClientType.MailTrap => _ => new MailtrapClient(smtpOptions.UserName, smtpOptions.Password),
        _ => _ => new MailKitClient(smtpOptions)
    };

    return services.AddTransient(mailClientFactory);
}
```

As mentioned above the below example enum can be used to switch the `EmailClientType`. This is an enum that can optionally be added to an application using `Audacia.Mail` to make it easier to configure different types. The exact values will depend on what email clients your project needs.
```csharp
    // The type of email client being used, e.g. basic SMTP, local SMTP, MailTrap, etc.
    public enum EmailClientType
    {
        // No SMTP client so no emails will be sent. Can be used for testing.
        None,

        // Use a general SMTP client.
        Smtp,

        // Use a local SMTP client.
        Local,

        // Use the MailTrap SMTP client.
        MailTrap,

        // Use SendGrid to send emails.
        SendGrid
    }
```

### Using IMailClient Interface In A Service.
Using the standard `IMailClient` interface you can create a service that will take in the chosen mail client, as seen above, as well as any information you need.

Below is an example of a password email service which takes in a `IMailClient`, `IHostingEnvironment` and as seen in the configuration a `EmailSenderDetails`. The `IMailClient` can be any of the above that inherits the `IMailClient` interface.
```csharp
public class PasswordEmailService : IPasswordEmailService
{
    private readonly IMailClient _mailClient;
    private readonly EmailSenderDetails _senderDetails;
    private readonly IHostingEnvironment _hostingEnvironment;

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordEmailService"/> class.
    /// </summary>
    /// <param name="mailClient">The <see cref="IMailClient"/> that has been setup in the pipeline for actually sending emails.</param>
    /// <param name="senderDetails">The details of the person that is sending the email.</param>
    /// <param name="hostingEnvironment">The environment that we are currently in.</param>
    public PasswordEmailService(
        IMailClient mailClient,
        EmailSenderDetails senderDetails,
        IHostingEnvironment hostingEnvironment)
    {
        _mailClient = mailClient;
        _senderDetails = senderDetails;
        _hostingEnvironment = hostingEnvironment;
    }

    /// <inheritdoc />
    public Task SendAsync(
        ApplicationUser user,
        Uri url,
        PasswordMode mode)
    {
        user = user ?? throw new ArgumentNullException(nameof(user));
        url = url ?? throw new ArgumentNullException(nameof(url));

        var email = new MailMessage(user.Email)
        {
            Sender = _senderDetails.Address,
            Subject = $"{mode.ToEnumDescriptionString()} your password",
            Body = $"{mode.ToEnumDescriptionString()} your password by <a href='{url}'>clicking here</a>.",
            Format = MailFormat.Html
        };

        if (!_hostingEnvironment.IsProduction())
        {
            email.Subject += $" ({_hostingEnvironment.EnvironmentName})";
        }

        return _mailClient.SendAsync(email);
    }
}
```

### Using IMailClient Interface in IoC pattern
Creating a MailClientHandelerFactory can enable to use different implementationsof IMailClientat runtime. This can be done based on a Http request custome header.

In order to set this up first create `MailClientHandlerFactory`
```csharp
namespace Audacia.Mail.Test.API;

public class MailClientHandlerFactory : IMailClientHandlerFactory
{
    private readonly SmtpOptions _smtpOptions;

    public MailClientHandlerFactory(
        SmtpOptions smtpOptions
        )
    {
        _smtpOptions = smtpOptions ?? throw new ArgumentNullException(nameof(smtpOptions));
    }

    public IMailClient CreateMailClient(HttpRequest request)
    {
        request.TryParseCustomHeaderValueIntoBoolean(
            _smtpOptions.DontForwardMessageToProviderCustomHeaderName ?? string.Empty,
            out bool dontForwardMessageToProvider
            );

        return dontForwardMessageToProvider
            ? (IMailClient)Activator.CreateInstance(typeof(NoopMailClient))
            : _smtpOptions.EmailClientType switch
            {
                EmailClientType.None => (IMailClient)Activator.CreateInstance(typeof(NoopMailClient)),
                _ => (IMailClient)Activator.CreateInstance(typeof(MailKitClient), _smtpOptions)
            };
    }
}
```

Extract `IMailClientHandlerFactory` from this class. Using DI add this interfcase as singleton.

An extension method can be created against `HttpRequest` which tryes to extract value from a given custom header name

```csharp
namespace Audacia.Mail.Test.API.Extensions;

public static class HttpRequestExtension
{
    public static bool TryParseCustomHeaderValueIntoBoolean(this HttpRequest request, string headerName, out bool headerValue)
    {
        if (request.Headers.TryGetValue(headerName, out var headerStringValue))
        {
            return bool.TryParse(headerStringValue, out headerValue);
        }

        return headerValue = false;
    }
}
```

Now we can use `IMailClientHandlerFactory` at controller level

```csharp
namespace Audacia.Mail.Test.API.Controllers;
[ApiController]
[Route("[controller]")]
public class MailController : Controller
{
    private readonly IMailService _mailService;
    private readonly IMailClientHandlerFactory _mailClientHandlerFactory;

    public MailController(
        IMailService mailService,
        IMailClientHandlerFactory mailClientHandlerFactory
        )
    {
        _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        _mailClientHandlerFactory = mailClientHandlerFactory ?? throw new ArgumentNullException(nameof(mailClientHandlerFactory));
    }

    [HttpPost(Name = "SendMail")]
    public async Task<IActionResult> SendMailAsync([FromBody]SendMailRequest sendMailRequest)
    {
        var mailClient = _mailClientHandlerFactory.CreateMailClient(HttpContext.Request);

        await _mailService.SendMailAsync(sendMailRequest, mailClient);

        return Ok();
    }
}
```

Thsi enables us to interact with multiple `IMailClient` implementations at runtime based on custom logic. A working example can be found under `Audacia.Mail.Test.API` project.

## Implementations

### `Audacia.Mail.Local`
Sends email to the local machine to be captured by a locally hosted SMTP server such as [Papercut](https://github.com/changemakerstudios/papercut) or [smtp4dev](https://github.com/rnwood/smtp4dev).
This library can automatically install (if needed) and start the specified SMTP server when debugging. Papercut is recommended as it doesn't require any configuration whereas smtp4dev needs authentication disabled in order to work.

This can be easily achieved by using the `DevMailClient` found in the `Audacia.Mail.Local`. This takes in a `ServerType`, a `port` (defaulted to 25) and a `defaultSender` (defaulted to null). This class overrides the `SendAsync` method from the `IMailClient` interface. In the example under Setting up an email client you can configure this `DevMailClient` by using the libraries enum `ServerType` and a port from the `smtpOptions` from the configuration.
```csharp
public DevMailClient(ServerType serverType, int port = 25, string defaultSender = null)
    : base(GetSettings(port))
{
    ServerType = serverType;
    DefaultSender = defaultSender;

    if (serverType == ServerType.Smtp4dev)
    {
        UserName = Guid.NewGuid().ToString();
        Password = Guid.NewGuid().ToString();
    }
}
```

The `DevMailClient` inherits the `MailKitClient` and the `GetSettings` method creates a new `SmtpSettings` object to send back to the `MailKitClient`.
```csharp
private static SmtpSettings GetSettings(int port)
{
    return new SmtpSettings
    {
        Host = "localhost",
        Port = port
    };
}
```

### `Audacia.Mail.MailKit`
This library uses standard SMTP protocol to send mails, implemented with MailKit. This should be used if you want to use a standard SMTP client.

MailKit uses the `Audacia.Mail` `SmtpSettings` class to set up the various properties for sending an email. This `SmtpSettings` can be configured below and using the `appsetting.json` like in the section [Configuring The Email Library](#configuring-the-email-library).
```csharp
public class SmtpSettings
{
    public SmtpSettings();

    public string UserName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string DefaultSender { get; set; }
}
```

You can set up a `MailKitClient` with the above `SmtpSettings`.
```csharp
public MailKitClient(SmtpSettings settings)
{
    if (settings == null) throw new ArgumentNullException(nameof(settings));

    Host = settings.Host;
    Port = settings.Port;
    UserName = settings.UserName;
    Password = settings.Password;
    DefaultSender = settings.DefaultSender;
}
```

This class has an extra method called `Connect` which will connect to a specified Host and Port and ensure it is Authenticated.
```csharp
public void Connect()
{
    if (!_client.IsConnected)
    {
        _client.Connect(Host, Port, SecureSocketOptions.None);
    }

    if (!_client.IsAuthenticated && _client.AuthenticationMechanisms.Any())
    {
        _client.Authenticate(UserName, Password);
    }
}
```

### `Audacia.Mail.Mailtrap`
Send mail to the Mailtrap server for testing purposes. Uses the MailKit SMTP implementation.
```csharp
public MailtrapClient(string username, string password)
    : base(GetSettings(username, password))
{
}
```

`MailtrapClient` also inherits `MailKitClient` and similar to the `DevMailClient` it creates a new `SmtpSettings` object with the private method `GetSettings`.

### `Audacia.Mail.SendGrid`
Send mail using the SendGrid API.

When creating a `SendGridClient` you need to pass down an apiKey which will be sent to the `SendGrid` api to create a sendGridClient.
```csharp
public SendGridClient(string apiKey)
{
    _client = new global::SendGrid.SendGridClient(apiKey);
}
```

### `Audacia.Mail.Log`
Writes an email to a delegate, to help with testing.

`Audacia.Mail` has an example of how to use this to log an email to the console.
```csharp
/// <summary>
/// Logs an email to the console.
/// </summary>
public class ConsoleLogMailClient : LogMailClient
{
    /// <summary>
    /// Initializes a new <see cref="ConsoleLogMailClient"/>.
    /// </summary>
    public ConsoleLogMailClient() : base(System.Console.WriteLine)
    {
    }
}
```

### `Audacia.Mail.Noop`
This client doesn't do anything with the email that is sent.

### `Audacia.Mail.Mandrill`
Send emails using the Mandrill (Mailchimp) API.

Mandrill is different to the other `IMailClient` implementations above as it uses a `HttpClient` to send api calls to the api address.

Due to this it has more configuration needed to be able to use it, if you are using Mandrill just for SMTP it is best to use MailKit instead as it needs less configuration to get working.

There is an extension within the Mandrill library called `AddMandrillClient` which will add the `MandrillClient` and the `MandrillService` to your `IServiceCollection` to allow for dependency injection.
as seen below.
```csharp
public static IServiceCollection AddMandrillClient(this IServiceCollection services, MandrillOptions options)
{
    return services
        .AddSingleton(options)
        .AddTransient<IMailClient, MandrillClient>()
        .AddHttpClient<IMandrillService, MandrillService>(client =>
        {
            client.BaseAddress = new Uri("https://mandrillapp.com/api/1.0/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).Services;
}
```

Mandrill uses `MandrillOptions` instead of the `SmtpSettings` to set up various properties for sending a Mandrill API `Post` request. Similarly to the `SmtpSettings` these can be configured using the
`appsetting.json` like in the section [Configuring The Email Library](#configuring-the-email-library).
```csharp
public class MandrillOptions
{
    public string FromEmail { get; set; }

    public string FromName { get; set; }

    public string ApiKey { get; set; }

    public bool Async { get; set; }
}
```

The Mandrill library also allows you to create template messages using the `MandrillTemplate` class as seen below.
```csharp
public class MandrillTemplate
{
    public string Name { get; set; }

    public string Content { get; set; }
}
```

```csharp
public async Task<bool?> SendTemplateMessageAsync(MailMessage message, string templateName, List<MandrillTemplate> templates = null)
{
    var mandrillMessage = new MandrillMailMessage(message);
    var messageRequest = templates != null ?
        new SendTemplateMessageRequest(_options.ApiKey, mandrillMessage, templates, templateName, _options.Async) :
        new SendTemplateMessageRequest(_options.ApiKey, mandrillMessage, templateName, _options.Async);
    using (var result = await SendPostRequestAsync($"messages/send-template{_outputFormat}", messageRequest))
    {
        return result.IsSuccessStatusCode;
    }
}
```

Mandrill also has the ability to use webhooks to receive information about email events as they occur. This uses the `MandrillWebhookProvider` class.

```csharp
public MandrillWebhookProvider(IMandrillService mandrillService, MandrillOptions options)
{
    _mandrillService = mandrillService;
    _options = options;
}
```

# Contributing
We welcome contributions! Please feel free to check our [Contribution Guidlines](https://github.com/audaciaconsulting/.github/blob/main/CONTRIBUTING.md) for feature requests, issue reporting and guidelines.