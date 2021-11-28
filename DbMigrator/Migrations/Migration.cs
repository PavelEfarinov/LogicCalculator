using System.Data;
using Dapper;

namespace DbMigrator.Migrations
{
	public abstract class Migration
	{
		public virtual string MigrationQuery { get; set; }

		public void ExecuteMigration(IDbConnection activeConnection)
		{
			activeConnection.Execute(MigrationQuery);
		}
	}
}
