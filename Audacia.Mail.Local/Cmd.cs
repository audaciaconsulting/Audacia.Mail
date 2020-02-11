using System;
using System.Diagnostics;

namespace Audacia.Mail.Local
{
	internal static class Cmd
	{
		public static bool Execute(string command, bool asAdmin)
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					UseShellExecute = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					FileName = "cmd.exe",
					Arguments = "/C " + command,
					Verb = asAdmin ? "runas" : null
				}
			};

			using (process)
			{
				Console.Write("cmd: ");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(command);
				Console.ResetColor();

				process.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
				process.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);

				process.Start();
				process.WaitForExit();

				return process.ExitCode == 0;
			}
		}
	}
}