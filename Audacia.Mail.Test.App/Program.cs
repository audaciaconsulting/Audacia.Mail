using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Audacia.Mail.Log;
using Audacia.Random.Extensions;

namespace Audacia.Mail.Test.App;

internal static class Program
{
    [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Email addresses should be lower case.")]
    private static async Task Main()
    {
        var localMailer = new LogMailClient((message) =>
        {
            Console.WriteLine(message);
        });

        using (localMailer)
        {
            var random = new System.Random();

            for (var iteration = 1; iteration <= 10; iteration++)
            {
                var forenames = new[]
                {
                    random.Boolean()
                        ? random.MaleForename()
                        : random.FemaleForename(),
                    random.Boolean()
                        ? random.MaleForename()
                        : random.FemaleForename()
                };

                var surnames = new[]
                {
                    random.Surname(),
                    random.Surname()
                };

                var message = new MailMessage
                {
                    Format = MailFormat.Html,
                    Sender = new MailAddress
                        {
                            Address = $"{forenames[0]}.{surnames[0]}@example.com".ToLowerInvariant(),
                            Name = $"{forenames[0]} {surnames[0]}"
                        },
                    Recipients =
                    {
                        new MailAddress
                        {
                            Address = $"{forenames[1]}.{surnames[1]}@example.com".ToLowerInvariant(),
                            Name = $"{forenames[1]} {surnames[1]}"
                        }
                    },
                    Subject = $"Test {iteration}: {random.Word()} {random.Word()}",
                    Body = $"<div><b>{random.Sentence()}</b></div>"
                };

                await SendMailAsync(localMailer, message, iteration);
            }

            await Task.Delay(int.MaxValue).ConfigureAwait(false);
        }
    }

    private static async Task SendMailAsync(LogMailClient localMailer, MailMessage message, int iteration)
    {
        await localMailer.SendAsync(message).ConfigureAwait(false);
        Console.WriteLine($"Sent email #{iteration}");
        await Task.Delay(1000).ConfigureAwait(false);
        iteration++;
    }
}