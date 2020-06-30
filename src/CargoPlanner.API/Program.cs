using System.Runtime.CompilerServices;
using CargoPlanner.API.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CargoPlanner.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<CargoPlannerContext>();
            context.Database.EnsureCreatedAsync().Wait();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureLogging(logging =>
                       {
                           logging.ClearProviders();
                           logging.AddConsole();
                           logging.AddDebug();
                       })
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.UseStartup<Startup>();
                       });
        }
    }
}