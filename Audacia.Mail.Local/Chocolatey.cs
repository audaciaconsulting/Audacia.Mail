using System;

namespace Audacia.Mail.Local
{
	internal static class Chocolatey
	{
		public static bool IsInstalled => Cmd.Execute("choco --version", false);

		public static void Install()
		{
			var success = Cmd.Execute(@"@""%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe"" -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command ""iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))"" && SET ""PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin""", true);
			if (!success) throw new PlatformNotSupportedException("Failed to install chocolatey.");

			Console.WriteLine("Successfully installed Chocolatey");
		}
	}
}