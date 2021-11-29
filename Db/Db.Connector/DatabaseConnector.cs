using System.Data;
using Db.Connector.Config;
using Npgsql;

namespace Db.Connector
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
