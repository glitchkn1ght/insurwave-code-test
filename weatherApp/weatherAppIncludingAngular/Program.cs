//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 21/10/2022 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherAppIncludingAngular
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using weatherApp;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((hostingContext, loggerConfig) =>
                 loggerConfig.ReadFrom.Configuration(hostingContext.Configuration));
    }
}
