using CommandLine;

namespace Logic.Client.CommandLineOptions
{
	public class ProgramStartOptions
	{
		[Option("fileName", Default = "userSessionFile.txt")]
		public string FileName { get; set; }
		[Option("url", Default = "http://localhost:5000")]
		public string Url { get; set; }
	}
}
