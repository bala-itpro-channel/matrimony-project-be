using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;

namespace Matrimony.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Inject logger
            var logger = NLogBuilder
                .ConfigureNLog("nlog.config")
                .GetCurrentClassLogger();
            try
            {
                logger.Info("Initializing application...");
                // Bootstrapping app
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                // Log the error on bootstrapping
                logger.Error(ex, "Application stopped because of exception.");
                throw ex;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //NLog 
                    webBuilder.UseNLog();
                });
    }
}
