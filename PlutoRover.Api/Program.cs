using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlutoRover.Api.Application;
using PlutoRover.Api.Infrastructure;
using ConfigurationProvider = PlutoRover.Api.Configuration.ConfigurationProvider;
using IConfigurationProvider = PlutoRover.Api.Configuration.IConfigurationProvider;

namespace PlutoRover.Api
{
    internal static class Program
    {
        private static void Main()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var commander = serviceProvider.GetService<Commander>();
            try
            {
                commander.Start();
            }
            catch (Exception e)
            {
                commander.HandleError(e);
            }
        }
        
        private static void ConfigureServices(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            services
                .AddLogging(configure => 
                    configure.SetMinimumLevel(LogLevel.Trace).AddConsole())
                .AddTransient<IReader, ConsoleReader>()
                .AddSingleton(configuration)
                .AddSingleton<IConfigurationProvider, ConfigurationProvider>()
                .AddSingleton<Map>()
                .AddSingleton<IRover, Rover>()
                .AddSingleton<Commander>();
        }
    }
}