using Microsoft.Extensions.Options;
using ReservasTucson.API.Support.Handlers;
using ReservasTucson.Domain.Support.Helpers;

namespace ReservasTucson.API.Support
{
    public static class LoggerExtension
    {
        public static string sectionKey = "Logging";
        public static string configurationKey = "AppLogger";

        /// <summary>
        /// Configura el Logger a los servicios
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddReservasTucsonLogger(this IServiceCollection services, IConfiguration configuration)
        {
            var loggerSettings = GetSettings(configuration);

            if (loggerSettings != null)
            {
                services.AddSingleton<ILoggerProvider, AppLoggerProvider>();

                services.AddLogging(
                    config => config
                    .ClearProviders()
                    .AddProvider(new AppLoggerProvider(loggerSettings))
                    .AddDebug());
            }

            return services;
        }

        /// <summary>
        /// Obtiene la configuracion para los loggs
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static LoggerSettings GetSettings(IConfiguration configuration)
        {
            try
            {
                // Configuración directa desde appsettings.json
                var loggerSettings = configuration.GetSection($"{sectionKey}:{configurationKey}").Get<LoggerSettings>();
                return loggerSettings;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener LoggerSettings: {ex.Message}");
                return null;
            }
        }
    }
}

