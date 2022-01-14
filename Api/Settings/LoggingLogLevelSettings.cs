using Microsoft.Extensions.Logging;

namespace Api.Settings;

public class LoggingLogLevelSettings
{
    public LogLevel Default { get; set; } = LogLevel.Error;
}