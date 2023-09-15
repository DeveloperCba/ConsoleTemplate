using Microsoft.Extensions.Configuration;

namespace ConsoleAppEslastic.Configurations
{
    public static class AppSettingsExtensions
    {
        public static IConfiguration GetConfigurationAppSettings()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Método responsável por obter Section do AppSettings
        /// </summary>
        /// <typeparam name="T">Informe o tipo da Lista de retorno (tem que ser uma classe)</typeparam>
        /// <param name="configuration">Informe o Configuration</param>
        /// <param name="name">Informe o nome da Section</param>
        /// <returns></returns>
        public static T GetAppSettings<T>(this IConfiguration configuration, string name)
        {
            return configuration!.GetRequiredSection(name).Get<T>();
        }
    }
}
