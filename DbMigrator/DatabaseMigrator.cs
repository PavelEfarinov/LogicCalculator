using System.Collections.Generic;
using DbConnector;
using DbMigrator.Config;
using DbMigrator.Migrations;
using Microsoft.Extensions.Logging;

namespace DbMigrator
{
	public sealed class DatabaseMigrator
	{
		private DatabaseConnector _connector;
		private readonly List<Migration> _migrations;

		private readonly ILogger<DatabaseMigrator> _logger;

		public DatabaseMigrator(ILogger<DatabaseMigrator> logger, DatabaseMigratorConfig config, DatabaseConnector connector)
		{
			_logger = logger;
			_connector = connector;
			_migrations = config.ServiceMigrations;
		}

		public void Migrate()
		{
			using var dbConnection = _connector.GetConnection();
			dbConnection.Open();

			foreach (var migration in _migrations)
			{
				_logger.LogInformation(
					"Executing db migration \"{MigrationName}\":\n\"{MigrationQuery}\"",
					migration.GetType().Name,
					migration.MigrationQuery);
				migration.ExecuteMigration(dbConnection);
			}
		}
	}
}
