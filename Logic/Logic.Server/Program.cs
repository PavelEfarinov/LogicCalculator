using System;
using System.Threading;
using System.Threading.Tasks;
using Db.Migrator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Logic.Server
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			Task.Run(() =>
			{
				var success = false;
				while(!success)
				{
					try
					{
						host.Services.GetService<DatabaseMigrator>()?.Migrate();
						success = true;
					}
					catch (Exception e)
					{
						Console.WriteLine($"Migration failed due to: {e}");
						Thread.Sleep(TimeSpan.FromSeconds(2));
					}
				}
			}).Wait(TimeSpan.FromSeconds(30));
			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
	}
}
