using Microsoft.Extensions.Logging;
using System.Runtime.Serialization;


namespace ReservasTucson.Domain.Support.Helpers
{
    public class LoggerSettings
    {
        /// <summary>
        /// The min log level that will be written to the console, defaults to <see cref="LogLevel.Information"/>.
        /// </summary>
        public LogLevel MinLogLevel { get; set; } = LogLevel.Information;

        /// <summary>
        /// Formato de fecha para la escritura
        /// </summary>
        public string DateTimeFormat { get; set; } = "dd-MM-yyyy HH:mm:ss";

        public LoggerType Type { get; set; }
        public long MaxFolderSize { get; set; }
        public string FilePath { get; set; }
        public string FolderPath { get; set; }
    }

    public enum LoggerType
    {
        [EnumMember(Value = "File")]
        FILE,
        [EnumMember(Value = "Console")]
        CONSOLE
    }
}
