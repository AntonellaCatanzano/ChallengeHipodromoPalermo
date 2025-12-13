using ReservasTucson.Domain.Support.Helpers;
using System.Diagnostics.CodeAnalysis;


namespace ReservasTucson.API.Support.Handlers
{
    public class AppLogger : ILogger
    {
        #region Dependencias
        protected readonly AppLoggerProvider _loggerProvider;
        private static object locker = new object();
        #endregion

        #region Constructor
        public AppLogger(
            [NotNull] AppLoggerProvider loggerProvider
            )
        {
            this._loggerProvider = loggerProvider;
        }
        #endregion

        #region Metodos
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            try
            {

                string logEventDate = DateTimeOffset.UtcNow.ToLocalTime().ToString(_loggerProvider._options.DateTimeFormat);
                string logEventLevel = logLevel.ToString();
                string logEventState = formatter(state, exception);
                string logEventStackTrace = exception != null ? exception.StackTrace : "";
                string logRecord = string.Format("{0} [{1}] {2} {3}",
                        "[" + logEventDate + "]",
                        logEventLevel,
                        logEventState,
                        logEventStackTrace);

                if (_loggerProvider._options.Type == LoggerType.FILE)
                {

                    var files = new DirectoryInfo(_loggerProvider._options.FolderPath).GetFiles().ToList();
                    long size = files.Sum(f => f.Length); // suma correctamente los tamaños

                    if (size > _loggerProvider._options.MaxFolderSize)
                    {
                        var oldestFile = files.OrderBy(f => f.LastWriteTime).FirstOrDefault();
                        if (oldestFile != null)
                        {
                            File.Delete(oldestFile.FullName);
                        }
                    }


                    var fullFilePath = Path.Combine(_loggerProvider._options.FolderPath, _loggerProvider._options.FilePath.Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyyMMdd")));


                    FileInfo fi = new FileInfo(fullFilePath);

                    lock (locker)
                    {
                        using var file = new FileStream(fullFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                        using var writer = new StreamWriter(file);
                        writer.WriteLine(logRecord);
                    }

                }
                else if (_loggerProvider._options.Type == LoggerType.CONSOLE)
                {
                    ConsoleColor originalColor = Console.ForegroundColor;

                    Console.ForegroundColor = originalColor;
                    Console.WriteLine(logRecord);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AppLogger: {ex}");
            }
        }
        #endregion
    }
}
