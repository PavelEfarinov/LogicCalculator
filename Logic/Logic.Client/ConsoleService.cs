using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Logic.Client.CommandLineOptions;
using Logic.Client.Grpc.Facades;
using Logic.Client.Grpc.Factories;
using Logic.Client.Helpers;
using Logic.Dto.Models;

namespace Logic.Client
{
	public class ConsoleService
	{
		private readonly SessionFileHelper _sessionFileHelper;
		private readonly string _grpcUrl;
		private UserSessionDto _currentSessionDto;
		private readonly SessionServiceFacade _sessionServiceFacade;
		private readonly NodeServiceFacade _nodeServiceFacade;
		private readonly Parser _parser;

		public ConsoleService(SessionServiceFacade sessionServiceFacade, SessionFileHelper sessionFileHelper,
			string grpcUrl, NodeServiceFacade nodeServiceFacade)
		{
			_sessionServiceFacade = sessionServiceFacade;
			_sessionFileHelper = sessionFileHelper;
			_grpcUrl = grpcUrl;
			_nodeServiceFacade = nodeServiceFacade;
			_parser = new Parser(with =>
			{
				with.CaseInsensitiveEnumValues = true;
				with.EnableDashDash = true;
			});
		}

		public async Task InitSession()
		{
			GrpcChannelFactory.Init(_grpcUrl);

			try
			{
				var userSession = _sessionFileHelper.GetSessionIfExists();
				Console.WriteLine($"Got session from file {userSession.SessionId}");
				_currentSessionDto = await _sessionServiceFacade.InitConnectionWithGuidAsync(userSession);
				Console.WriteLine($"Got session from server {_currentSessionDto.SessionId}");
			}
			catch (FileNotFoundException)
			{
				_currentSessionDto = await _sessionServiceFacade.InitConnectionAsync();
				Console.WriteLine($"Got new session {_currentSessionDto.SessionId}");
			}

			_sessionFileHelper.SaveSession(_currentSessionDto);
		}

		public void Run()
		{
			var shouldRun = true;
			while(shouldRun)
			{
				var userInput = Console.ReadLine()?.Split(' ');
				var result = _parser.ParseArguments<AddOptions, LinkOptions, ShowOptions, SetOptions, EndOptions, AllOptions>(userInput);
				result
					.WithParsed<AddOptions>(options =>
					{
						Console.WriteLine(_nodeServiceFacade.AddNode(options, _currentSessionDto.SessionId));
					}).WithParsed<LinkOptions>(options =>
					{
						Console.WriteLine(_nodeServiceFacade.LinkNodes(options, _currentSessionDto.SessionId));
					}).WithParsed<ShowOptions>(options =>
					{
						Console.WriteLine(_nodeServiceFacade.GetNodeInfo(options, _currentSessionDto.SessionId));
					}).WithParsed<SetOptions>(options =>
					{
						Console.WriteLine(_nodeServiceFacade.SetNodeValue(options, _currentSessionDto.SessionId));
					}).WithParsed<AllOptions>(options =>
					{
						Console.WriteLine(_nodeServiceFacade.GetAllNodesInfo(_currentSessionDto.SessionId));
					}).WithParsed<EndOptions>(options =>
					{
						shouldRun = false;
					})
					.WithNotParsed(x =>
					{
						var text = HelpText.AutoBuild(result);
						Console.WriteLine(text);
					});
			}
		}
	}
}
