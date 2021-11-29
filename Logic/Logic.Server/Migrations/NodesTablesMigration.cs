using Db.Migrator.Migrations;

namespace Logic.Server.Migrations
{
	public class NodesTablesMigration : Migration
	{
		public override string MigrationQuery =>
			@"CREATE TABLE operator_types (type_name text primary key);
			CREATE TABLE logic_nodes (node_name text primary key, node_value bool, user_id uuid not null REFERENCES user_sessions, node_type text not null REFERENCES operator_types);
			CREATE TABLE logic_nodes_links (node_name_src text REFERENCES logic_nodes, node_name_dest text REFERENCES logic_nodes);
			INSERT INTO operator_types (type_name) values ('CONST'), ('NOT'), ('AND'), ('OR')";
	}
}
