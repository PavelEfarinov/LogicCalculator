using System;
using Dapper;
using Db.Connector;
using Logic.Server.Models;
using Microsoft.Extensions.Logging;

namespace Logic.Server.Repositories
{
	public class UserSessionsRepository
	{
		private readonly ILogger<UserSessionsRepository> _logger;
		private readonly DatabaseConnector _connector;
		private readonly string sessionTable = "user_sessions";

		public UserSessionsRepository(ILogger<UserSessionsRepository> logger, DatabaseConnector connector)
		{
			_logger = logger;
			_connector = connector;
		}

		public Guid CreateNewUser()
		{
			_logger.LogInformation("Creating new user");
			using var connection = _connector.GetConnection();
			var query = $@"INSERT INTO {sessionTable} (last_login_date) VALUES (@CurrentTime) RETURNING user_id";
			connection.Open();
			var result = connection.QueryFirst<Guid>(query, new {CurrentTime = DateTimeOffset.UtcNow});
			_logger.LogInformation("Created new user with id {Id}", result);
			return result;
		}

		public UserSession GetUserBySession(Guid userId)
		{
			_logger.LogInformation("Reading user with id {Id}", userId);
			using var connection = _connector.GetConnection();
			var query = $@"SELECT * FROM {sessionTable} WHERE user_id = @Id";
			connection.Open();
			return connection.QueryFirst<UserSession>(query, new{Id = userId});
		}

		public void UpdateLastLoginTime (Guid userId)
		{
			_logger.LogInformation("Reading user with id {Id}", userId);
			using var connection = _connector.GetConnection();
			var query = $@"UPDATE {sessionTable} SET last_login_date = @CurrentTime WHERE user_id = @Id";
			connection.Open();
			connection.Execute(query, new{Id = userId, CurrentTime = DateTimeOffset.UtcNow});
		}
	}
}
