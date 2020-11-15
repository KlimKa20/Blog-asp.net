using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Events;
using WebApplication4.Domain.Core;
using WebApplication4.Infrastructure.Data;
using Azure.Identity;

namespace WebApplication4
{
    public class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<Profile>>();
                    var config = services.GetRequiredService<IConfiguration>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .WriteTo.File(config["all-logs"])
                        .WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
                        .WriteTo.File(config["error-logs"]))
                        .WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
                        .WriteTo.Console())
                        .CreateLogger();
                    await RoleInitializer.InitializeAsync(userManager, rolesManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

//.ConfigureAppConfiguration((context, config) =>
//{
//var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
//config.AddAzureKeyVault(
//keyVaultEndpoint,
//new DefaultAzureCredential());
//})

//.ConfigureAppConfiguration((context, config) =>
//{
//var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUriwq"));
//config.AddAzureKeyVault(
//keyVaultEndpoint,
//new DefaultAzureCredential());
//})
                  //.ConfigureAppConfiguration((context, config) =>
                  //{
                  //var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
                  //config.AddAzureKeyVault(
                  //keyVaultEndpoint,
                  //new DefaultAzureCredential());
                  //})

                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder.UseStartup<Startup>();
                  })
                        .UseSerilog();
    }
}
