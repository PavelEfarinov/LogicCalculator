using System;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Logic.Client.CommandLineOptions;
using Logic.Client.Grpc.Facades;
using Logic.Client.Helpers;

namespace Logic.Client
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var result = Parser.Default.ParseArguments<ProgramStartOptions>(args);
			result.WithParsed(options =>
			{
				var sessionService = new SessionFileHelper(options.FileName);
				var sessionFacade = new SessionServiceFacade();
				var nodeFacade = new NodeServiceFacade();
				var consoleService = new ConsoleService(sessionFacade, sessionService, options.Url, nodeFacade);
				consoleService.InitSession().ConfigureAwait(false).GetAwaiter().GetResult();
				consoleService.Run();

			}).WithNotParsed(x =>
			{
				var text = HelpText.AutoBuild(result);
				Console.WriteLine(text);
			});
		}
	}
}
