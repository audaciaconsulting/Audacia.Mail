using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Audacia.Mail.Local
{
	/// <summary>
	/// Class for launching local SMTP software such as Papercut.
	/// </summary>
	internal static class Launcher
	{
		/// <summary>
		/// Launches local SMTP software.
		/// </summary>
		/// <param name="name">Name of software to launch.</param>
		/// <returns>A <see cref="Task"/> representing completed task.</returns>
		/// <exception cref="PlatformNotSupportedException">Local SMTP software as defined by <paramref name="name"/> was not installed locally and failed to install via Chocolatey.</exception>
		public static async Task LaunchAsync(string name)
		{
            if (Process.GetProcessesByName(name).Length > 0) { return; }
			
            if (await StartProcessAsync(name).ConfigureAwait(false)) { return; }

            if (!Chocolatey.IsInstalled)
			{
				Chocolatey.Install();
			}

			var installed = Cmd.Execute($"choco install {name} -y", CommandPromptPrivilege.Admin);
            if (!installed)
            {
                throw new PlatformNotSupportedException($"Failed to install {name}.");
            }

			await StartProcessAsync(name).ConfigureAwait(false);
		}

		/// <summary>
		/// Execute command to launch software.
		/// </summary>
		/// <param name="name">Name of software to launch.</param>
		/// <returns>A <see cref="Task"/> representing the completed task.</returns>
		/// <exception cref="InvalidOperationException">Software failed to start.</exception>
		private static async Task<bool> StartProcessAsync(string name)
		{
			if (!Cmd.Execute(name, CommandPromptPrivilege.Standard)) { return false; }

            await StartProcessIterationsAsync(name).ConfigureAwait(false);

			return true;
		}

        private static async Task StartProcessIterationsAsync(string name)
        {
            var iterations = 0;
            const int delay = 1000;
            Process[] processes;

            do
            {
                if (iterations == 100)
                {
                    throw new InvalidOperationException($"Failed to start {name}.");
                }

                iterations++;
                await Task.Delay(delay).ConfigureAwait(false);
                Console.WriteLine($"Waiting for {name}, " + delay * iterations + "ms");
                processes = Process.GetProcessesByName(name);
            }
            while (processes.Length == 0 || processes[0].MainWindowHandle == IntPtr.Zero);
		}
    }
}