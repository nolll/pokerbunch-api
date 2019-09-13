using Microsoft.Extensions.Logging;

namespace Api.Settings
{
    public class LoggingSettings
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Error;
        public LoggingLoggersSettings Loggers { get; set; }
    }
}