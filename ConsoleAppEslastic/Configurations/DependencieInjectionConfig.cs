using ConsoleAppEslastic.Data;
using ConsoleAppEslastic.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace ConsoleAppEslastic.Configurations
{
    public static class DependencieInjectionConfig
    {
        public static ServiceProvider ConfigureService()
        {
            var configuration = AppSettingsExtensions.GetConfigurationAppSettings();

            var appSettings = configuration.GetAppSettings<ElasticSettings>(nameof(ElasticSettings));

            var serviceProvider = new ServiceCollection()

                .AddLogging(options =>
                {
                    options.AddConsole();
                    options.AddDebug();
                })
                .Configure<ElasticSettings>(configuration.GetSection(nameof(ElasticSettings)))
                //.AddHttpClient()
                //.AddHttpContextAccessor()
                //.AddScoped<IElasticRepository<,>, ElasticRepository<,>>()
                ;

            ServiceProvider provider = serviceProvider.BuildServiceProvider();
            return provider;
        }
    }
}

/* PACKAGE CONFIGURATION

Install-Package Microsoft.Extensions.Configuration
Install-Package Microsoft.Extensions.Configuration.Json
Install-Package Microsoft.Extensions.Configuration.CommandLine
Install-Package Microsoft.Extensions.Configuration.Binder
Install-Package Microsoft.Extensions.Configuration.EnvironmentVariables 
Install-Package Microsoft.Extensions.Configuration.FileExtensions
Install-Package Microsoft.Extensions.Configuration.Json
Install-Package Microsoft.Extensions.DependencyInjection
Install-Package Microsoft.Extensions.DependencyInjection.Abstractions
Install-Package Microsoft.Extensions.Logging
Install-Package Microsoft.Extensions.Logging.Console
Install-Package Microsoft.Extensions.Logging.Debug
Install-Package Microsoft.Extensions.Options

*/