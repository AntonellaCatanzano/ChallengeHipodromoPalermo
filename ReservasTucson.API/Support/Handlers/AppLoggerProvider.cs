using ReservasTucson.Domain.Support.Helpers;

namespace ReservasTucson.API.Support.Handlers
{
    [ProviderAlias("AppLogger")]
    public class AppLoggerProvider : ILoggerProvider
    {
        #region Dependencias
        public readonly LoggerSettings _options;
        #endregion

        #region Constructor
        public AppLoggerProvider(LoggerSettings options)
        {
            _options = options;

            if (_options.Type == LoggerType.FILE && !string.IsNullOrEmpty(_options.FolderPath))
            {
                try
                {
                    if (!Directory.Exists(_options.FolderPath))
                    {
                        Directory.CreateDirectory(_options.FolderPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creando carpeta de logs: {ex.Message}");
                }
            }
        }
        #endregion

        #region Metodos
        public ILogger CreateLogger(string categoryName)
        {
            return new AppLogger(this);
        }

        public void Dispose()
        {
            
        }
        #endregion
    }
}

