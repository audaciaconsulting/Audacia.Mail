using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Audacia.Mail.Local;
using Audacia.Random.Extensions;

namespace Audacia.Mail.Test.App
{
	internal static class Program
	{
		[SuppressMessage("ReSharper", "CA1308", Justification = "Strings are not being normalised.")]
		private static async Task Main()
		{
			var localMailer = new DevMailClient(ServerType.Papercut);

			using (localMailer)
			{
				var random = new System.Random();
				var iteration = 1;

				while (iteration <= 10)
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
						Sender =
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
						Body = "<img src=\"https://www.audacia.co.uk/media/pkenoobu/audacia-logo-circle-blue.png\">"
						       + $"<div><b>{random.Sentence()}</b></div>"
					};

					await localMailer.SendAsync(message).ConfigureAwait(false);
					Console.WriteLine($"Sent email #{iteration}");
					await Task.Delay(1000).ConfigureAwait(false);
					iteration++;
				}

				await Task.Delay(int.MaxValue).ConfigureAwait(false);
			}
		}
	}
}