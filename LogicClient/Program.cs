using System.Threading.Tasks;
using LogicClient.Grpc.Facades;
using LogicClient.Helpers;

namespace LogicClient
{
	class Program
	{
		static async Task Main(string[] args)
		{
			// todo move to env
			var sessionService = new SessionFileHelper("userSessionFile.txt");
			var sessionFacade = new SessionServiceFacade();
			var consoleService = new ConsoleService(sessionFacade, sessionService, "http://localhost:5000");
			await consoleService.InitSession();
			consoleService.Run();
		}
	}
}
