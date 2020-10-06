using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace OnlineAuction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File("Logs\\all-logs.txt")
                .WriteTo.Logger(lc=> lc
                    .Filter.ByIncludingOnly(le=> le.Level == LogEventLevel.Error)
                    .WriteTo.File("Logs\\error-logs.txt"))
                .WriteTo.Logger(lc=>lc
                    .Filter.ByIncludingOnly(le=>le.Level == LogEventLevel.Error)
                    .WriteTo.Console())
                .CreateLogger();
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}