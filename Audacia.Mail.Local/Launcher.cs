using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Audacia.Mail.Local
{
	internal static class Launcher
	{
		public static async Task LaunchAsync(string name)
		{
			var processes = Process.GetProcessesByName(name);
			if (processes.Length > 0) return;

			var launchSuccessful = await StartProcessAsync(name).ConfigureAwait(false);
			if (launchSuccessful) return;

			if (!Chocolatey.IsInstalled)
			{
				Chocolatey.Install();
			}

			var installed = Cmd.Execute($"choco install {name} -y", true);
			if (!installed) throw new PlatformNotSupportedException($"Failed to install {name}.");

			await StartProcessAsync(name).ConfigureAwait(false);
		}

		private static async Task<bool> StartProcessAsync(string name)
		{
			var launchSuccessful = Cmd.Execute(name, false);
			if (!launchSuccessful) return false;

			var iterations = 0;
			const int delay = 1000;
			Process[] processes;

			do
			{
				if (iterations == 100) throw new InvalidOperationException($"Failed to start {name}.");

				iterations++;
				await Task.Delay(delay).ConfigureAwait(false);
				Console.WriteLine($"Waiting for {name}, " + delay * iterations + "ms");
				processes = Process.GetProcessesByName(name);
			}
            while (processes.Length == 0 || processes[0].MainWindowHandle == IntPtr.Zero);

			return true;
		}
	}
}