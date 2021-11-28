using System;
using LogicServer.Helpers;
using Microsoft.Extensions.Logging;

namespace LogicServer.Services
{
	public class UserSessionService
	{
		private readonly UserSessionsRepository _sessionsRepository;
		private readonly ILogger<UserSessionService> _logger;

		public UserSessionService(ILogger<UserSessionService> logger, UserSessionsRepository sessionsRepository)
		{
			_logger = logger;
			_sessionsRepository = sessionsRepository;
		}

		public bool UserSessionExists(string sessionId)
		{
			try
			{
				_logger.LogInformation("Checking existence of session {SessionId}", sessionId);
				_sessionsRepository.GetUserBySession(Guid.Parse(sessionId));
				_sessionsRepository.UpdateLastLoginTime(Guid.Parse(sessionId));
				return true;
			}
			catch (Exception e)
			{
				_logger.LogWarning("Could not complete request {Exception}", e);
				return false;
			}
		}

		public Guid GenerateNewUserSession()
		{
			_logger.LogInformation("Generating new session");
			return _sessionsRepository.CreateNewUser();
		}
	}
}
