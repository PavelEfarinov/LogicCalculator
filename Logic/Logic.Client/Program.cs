using System.Threading.Tasks;
using Logic.Client.Grpc.Facades;
using Logic.Client.Helpers;

namespace Logic.Client
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
