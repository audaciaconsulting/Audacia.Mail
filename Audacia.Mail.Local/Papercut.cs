using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Audacia.Mail.Local
{
	internal static class Papercut
	{
		public static async Task LaunchAsync()
		{
			var processes = Process.GetProcessesByName("papercut");
			if (processes.Length > 0) return;

			var launchSuccessful = await LaunchPapercutAsync().ConfigureAwait(false);
			if (launchSuccessful) return;

			if (!Chocolatey.IsInstalled)
				Chocolatey.Install();

			var installed = Cmd.Execute("choco install papercut -y");
			if (!installed) throw new PlatformNotSupportedException("Failed to install papercut.");

			await LaunchPapercutAsync().ConfigureAwait(false);
		}

		private static async Task<bool> LaunchPapercutAsync()
		{
			var launchSuccessful = Cmd.Execute("papercut");
			if (!launchSuccessful) return false;

			var iterations = 0;
			const int delay = 1000;
			Process[] processes;

			do
			{
				if (iterations == 100) throw new InvalidOperationException("Failed to start papercut.");

				iterations++;
				await Task.Delay(delay).ConfigureAwait(false);
				Console.WriteLine("Waiting for papercut, " + delay * iterations + "ms");
				processes = Process.GetProcessesByName("papercut");
			}
            while (processes.Length == 0 || processes[0].MainWindowHandle == IntPtr.Zero);

			return true;
		}
	}
}