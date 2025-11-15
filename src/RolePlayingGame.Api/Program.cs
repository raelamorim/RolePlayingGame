using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace RolePlayingGame.Api
{
	[ExcludeFromCodeCoverage]
	public partial class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog() // mantém seu Serilog
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}