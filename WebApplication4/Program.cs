using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog_project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication4.Models;
using WebApplication4.Data;
using NLog.Web;
using NLog;

using Serilog;
using Serilog.Events;

namespace WebApplication4
{
    public class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File("Logs\\all-logs.txt")
                .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
                .WriteTo.File("Logs\\error-logs.txt"))
                .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
                .WriteTo.Console())
                .CreateLogger();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<Profile>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
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
      .ConfigureWebHostDefaults(webBuilder =>
      {
          webBuilder.UseStartup<Startup>();
      })
            .UseSerilog();
    }
}


//namespace WebApplication4
//{
//    public class Program
//    {
//        [STAThread]
//        public static async Task Main(string[] args)
//        {



//            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
//            try
//            {
//                logger.Debug("init main");
//                var host = CreateHostBuilder(args).Build();
//                using (var scope = host.Services.CreateScope())
//                {
//                    var services = scope.ServiceProvider;
//                    try
//                    {
//                        var userManager = services.GetRequiredService<UserManager<Profile>>();
//                        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//                        await RoleInitializer.InitializeAsync(userManager, rolesManager);
//                    }
//                    catch (Exception ex)
//                    {
//                        //var logger = services.GetRequiredService<ILogger<Program>>();
//                        //logger.LogError(ex, "An error occurred while seeding the database.");
//                    }
//                }
//                host.Run();
//            }
//            catch (Exception exception)
//            {
//                //NLog: catch setup errors
//                logger.Error(exception, "Stopped program because of exception");
//                throw;
//            }
//            finally
//            {
//                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
//                NLog.LogManager.Shutdown();
//            }



//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//      .ConfigureWebHostDefaults(webBuilder =>
//      {
//          webBuilder.UseStartup<Startup>();
//      })
//      .ConfigureLogging(logging =>
//      {
//          logging.ClearProviders();
//          logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
//      })
//      .UseNLog();
//    }
//}