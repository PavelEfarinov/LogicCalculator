using DbMigrator.Migrations;

namespace LogicServer.Migrations
{
	public class UserTablesMigration : Migration
	{
		public override string MigrationQuery => "CREATE TABLE user_sessions (user_id uuid primary key default gen_random_uuid(), last_login_date timestamp without time zone)";
	}
}
