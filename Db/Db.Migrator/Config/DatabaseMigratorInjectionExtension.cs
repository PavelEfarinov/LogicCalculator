using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Db.Migrator.Config
{
	public static class DatabaseMigratorInjectionExtension
	{
		public static IServiceCollection AddDatabaseMigrator(this IServiceCollection services)
		{
			services.TryAddSingleton<DatabaseMigrator>();
			return services;
		}

		public static IServiceCollection AddDatabaseMigratorConfig(this IServiceCollection services, DatabaseMigratorConfig config)
		{
			services.TryAddSingleton(config);
			return services;
		}
	}
}
