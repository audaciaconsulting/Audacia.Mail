using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Audacia.Mail.Local
{
	internal static class Smtp4dev
	{
		public static async Task LaunchAsync()
		{
			var processes = Process.GetProcessesByName("smtp4dev");
			if (processes.Length > 0) return;

			var launchSuccessful = await LaunchSmtp4devAsync().ConfigureAwait(false);
			if (launchSuccessful) return;

			if (!Chocolatey.IsInstalled)
				Chocolatey.Install();

			var installed = Cmd.Execute("choco install smtp4dev -y");
			if (!installed) throw new PlatformNotSupportedException("Failed to install papercut.");

			await LaunchSmtp4devAsync().ConfigureAwait(false);
		}

		private static async Task<bool> LaunchSmtp4devAsync()
		{
			var launchSuccessful = Cmd.Execute("smtp4dev");
			if (!launchSuccessful) return false;

			var iterations = 0;
			const int delay = 1000;
			Process[] processes;

			do
			{
				if (iterations == 100) throw new InvalidOperationException("Failed to start papercut.");

				iterations++;
				await Task.Delay(delay).ConfigureAwait(false);
				Console.WriteLine("Waiting for smtp4dev, " + delay * iterations + "ms");
				processes = Process.GetProcessesByName("smtp4dev");
			}
            while (processes.Length == 0 || processes[0].MainWindowHandle == IntPtr.Zero);

			return true;
		}
	}
}