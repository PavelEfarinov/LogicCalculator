using System.Threading.Tasks;
using Grpc.Core;
using Logic.Proto;
using Microsoft.Extensions.Logging;

namespace Logic.Server.Services.Grpc
{
	public class SessionConnectorService : SessionConnector.SessionConnectorBase
	{
		private readonly ILogger<SessionConnectorService> _logger;
		private readonly UserSessionService _sessionService;

		public SessionConnectorService(ILogger<SessionConnectorService> logger, UserSessionService sessionService)
		{
			_logger = logger;
			_sessionService = sessionService;
		}

		public override Task<ConnectionReply> InitConnection(NewConnectionRequest request, ServerCallContext context)
		{
			return Task.FromResult(new ConnectionReply
				{
					UserId = _sessionService.GenerateNewUserSession().ToString()
				}
			);
		}

		public override Task<ConnectionReply> InitConnectionWithGuid(ConnectionRequest request,
			ServerCallContext context)
		{
			return Task.FromResult(new ConnectionReply
			{
				UserId = _sessionService.UserSessionExists(request.UserId)
					? request.UserId
					: _sessionService.GenerateNewUserSession().ToString()
			});
		}
	}
};
