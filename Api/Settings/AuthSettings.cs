namespace Api.Settings;

public class AuthSettings
{
    public string Secret { get; set; } = "";
    public AuthOverrideSettings Override { get; set; } = new();
}