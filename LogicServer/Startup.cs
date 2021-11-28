using System.Collections.Generic;
using DbConnector.Config;
using DbMigrator.Config;
using DbMigrator.Migrations;
using LogicServer.Helpers;
using LogicServer.Migrations;
using LogicServer.Services;
using LogicServer.Services.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogicServer
{
	public class Startup
	{
		private readonly IConfiguration _environmentVariables = new ConfigurationBuilder().AddEnvironmentVariables().Build();

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddGrpc();
			services.AddDatabaseMigrator().AddDatabaseMigratorConfig(new DatabaseMigratorConfig()
				{
					ServiceMigrations = new List<Migration>
					{
						new UserTablesMigration(),
						new NodesTablesMigration()
					}
				})
				.AddDatabaseConnector()
				.AddDatabaseConnectorConfig(new DatabaseConnectorConfig
				{
					ConnectionString = _environmentVariables.GetValue<string>("PG_CONNECTION_STRING")
				});
			services.AddSingleton<UserSessionsRepository>();
			services.AddSingleton<UserSessionService>();
			services.AddSingleton<NodesRepository>();
			services.AddSingleton<NodesService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGrpcService<SessionConnectorService>();
				endpoints.MapGrpcService<NodesConnectorService>();

				endpoints.MapGet("/",
					async context =>
					{
						await context.Response.WriteAsync(
							"Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
					});
			});
		}
	}
}
