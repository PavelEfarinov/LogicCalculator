using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Db.Connector.Config
{
	public static class DatabaseConnectorInjectionExtension
	{
		public static IServiceCollection AddDatabaseConnector(this IServiceCollection services)
		{
			services.TryAddSingleton<DatabaseConnector>();
			return services;
		}

		public static IServiceCollection AddDatabaseConnectorConfig(this IServiceCollection services, DatabaseConnectorConfig config)
		{
			services.TryAddSingleton(config);
			return services;
		}
	}
}
