namespace Api.Services;

public class Environment
{
    private readonly string _hostName;

    public Environment(string hostName)
    {
        _hostName = hostName;
    }

    public bool IsDevMode => IsDevModeAdmin || IsDevModePlayer;
    public bool IsDevModeAdmin => IsLocal && Contains("api-admin");
    public bool IsDevModePlayer => IsLocal && Contains("api-player");
    public bool IsAnyTest => IsDev || IsTest || IsStage;
    private bool IsDev => EndsWith("pokerbunch.local") || EndsWith("pokerbunch.lan") || StartsWith("192.168.1");
    private bool IsTest => EndsWith("homeip.net");
    private bool IsStage => EndsWith("staging.pokerbunch.com");
    private bool IsLocal => EndsWith(".local") || EndsWith(".lan");
    private bool StartsWith(string s) => _hostName.StartsWith(s);
    private bool EndsWith(string s) => _hostName.EndsWith(s);
    private bool Contains(string s) => _hostName.Contains(s);
}