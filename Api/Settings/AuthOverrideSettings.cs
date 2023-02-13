namespace Api.Settings;

public class AuthOverrideSettings
{
    public bool Enabled { get; set; } = false;
    public string AdminUserName { get; set; } = "";
    public string PlayerUserName { get; set; } = "";
}