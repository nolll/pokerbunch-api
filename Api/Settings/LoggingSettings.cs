﻿namespace Api.Settings;

public class LoggingSettings
{
    public LoggingLogLevelSettings LogLevel { get; set; } = new();
    public LoggingLoggersSettings Loggers { get; set; } = new();
}