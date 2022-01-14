namespace Api.Settings;

public class AppSettings
{
    public string Version { get; set; }
    public UrlSettings Urls { get; set; }
    public AuthSettings Auth { get; set; }
    public SqlSettings Sql { get; set; }
    public EmailSettings Email { get; set; }
    public LoggingSettings Logging { get; set; }
    public ErrorSettings Error { get; set; }
}