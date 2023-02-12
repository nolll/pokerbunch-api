namespace Core;

public class Settings : ISettings
{
    public string InvitationSecret { get; }

    public Settings(string invitationSecret)
    {
        InvitationSecret = invitationSecret;
    }
}