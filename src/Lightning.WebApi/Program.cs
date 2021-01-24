using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lightning.Core.Configs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

namespace Lightning.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) 
        {
            // var config = new ConfigurationBuilder()
            //     .AddJsonFile("appsettings.json", true, true)
            //     .AddYamlFile("appsettings.yml", true, true)
            //     .AddEnvironmentVariables()
            //     .AddCommandLine(args);
        
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((ctx, builder) => 
                {
                    builder.Sources.Clear();
                    var env = ctx.HostingEnvironment;
                    builder.AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                        .AddYamlFile("appsettings.yml", true, true)
                        .AddYamlFile($"appsettings.{env.EnvironmentName}.yml", true, true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .UseSerilog((ctx, builder) => 
                {
                    builder.ReadFrom.Configuration(ctx.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
