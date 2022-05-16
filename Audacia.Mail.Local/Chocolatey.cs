using System;

namespace Audacia.Mail.Local
{
	/// <summary>
	/// Class for using Chocolatey commands.
	/// </summary>
	internal static class Chocolatey
	{
		/// <summary>
		/// Gets a value indicating whether Chocolatey is installed.
		/// </summary>
		public static bool IsInstalled => Cmd.Execute("choco --version", CommandPromptPrivilege.Standard);

		/// <summary>
		/// Method for installing Chocolatey.
		/// </summary>
		/// <exception cref="PlatformNotSupportedException">Chocolatey failed to install.</exception>
		public static void Install()
		{
			var success = Cmd.Execute(@"@""%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe"" -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command ""iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))"" && SET ""PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin""", CommandPromptPrivilege.Admin);
            if (!success)
            {
                throw new PlatformNotSupportedException("Failed to install chocolatey.");
            }

			Console.WriteLine("Successfully installed Chocolatey");
		}
	}
}