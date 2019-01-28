using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
namespace geektimedownload
{
	class Program
	{
		static void Main(string[] args)
		{
			var _host = new HostBuilder()

				.ConfigureAppConfiguration((hostBuilderContext, config) =>
				{
					config.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "configs"))
					.AddJsonFile("appsettings.json", optional: false);

					var builder = config.Build();
					//var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
					var courseDir = builder.GetValue<string>("GeektimeSetting:CoursePath");
					config.Download(Path.Combine(courseDir));
				}).Build();

			_host.Register().Run();
		}


	 
	}

	public static class HostBuilderExtension
	{
		 

		public static IHost Register(this IHost host)
		{
			var applicationLifetime = host.Services.GetService<IApplicationLifetime>();
			applicationLifetime.ApplicationStopping.Register(() =>
			{
				Console.WriteLine("Server is closed...");
			});	 
			return host;
		}

	}
}
