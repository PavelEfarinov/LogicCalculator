using System.Data;
using DbConnector.Config;
using Npgsql;

namespace DbConnector
{
	public class DatabaseConnector
	{
		private readonly string _connectionString;

		public DatabaseConnector(DatabaseConnectorConfig config)
		{
			_connectionString = config.ConnectionString;
		}

		public IDbConnection GetConnection()
		{
			return new NpgsqlConnection(_connectionString);
		}
	}
}
