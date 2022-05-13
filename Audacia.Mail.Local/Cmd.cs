using System;
using System.Diagnostics;

namespace Audacia.Mail.Local
{
	/// <summary>
	/// Class for executing command line commands.
	/// </summary>
	internal static class Cmd
	{
		/// <summary>
		/// Method for executing command.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <param name="privilege">Indicates whether to execute command from elevated command line window.</param>
		/// <returns><see cref="bool"/> indicating success.</returns>
		public static bool Execute(string command, CommandPromptPrivilege privilege)
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					UseShellExecute = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					FileName = "cmd.exe",
					Arguments = "/C " + command,
					Verb = privilege == CommandPromptPrivilege.Admin ? "runas" : null
				}
			};

			using (process)
            {
                RunProcess(process, command);

				return process.ExitCode == 0;
			}
		}

        private static void RunProcess(Process process, string command)
        {
            Console.Write("cmd: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(command);
            Console.ResetColor();

            process.OutputDataReceived += (_, e) => Console.WriteLine(e.Data);
            process.ErrorDataReceived += (_, e) => Console.WriteLine(e.Data);

            process.Start();
            process.WaitForExit();
        }
	}

    /// <summary>
	/// Enum for command prompt options.
	/// </summary>
    internal enum CommandPromptPrivilege
    {
		/// <summary>
		/// Normal command prompt.
		/// </summary>
		Standard,

		/// <summary>
		/// Elevated command prompt.
		/// </summary>
		Admin
    }
}