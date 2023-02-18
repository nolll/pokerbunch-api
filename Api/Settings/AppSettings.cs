namespace Api.Settings;

public class AppSettings
{
    // todo: Cleanup and flatten for simplicity
    public string Version { get; set; } = "";
    public UrlSettings Urls { get; set; } = new();
    public AuthSettings Auth { get; set; } = new();
    public EmailSettings Email { get; set; } = new();
    public LoggingSettings Logging { get; set; } = new();
    public ErrorSettings Error { get; set; } = new();
    public string InvitationSecret { get; set; } = "";
}