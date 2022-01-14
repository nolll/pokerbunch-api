namespace Core.Services;

public class RegistrationMessage : IMessage
{
    private readonly string _loginUrl;
    public string Subject => "Poker Bunch Registration";
    public string Body => string.Format(BodyFormat, _loginUrl);

    public RegistrationMessage(string loginUrl)
    {
        _loginUrl = loginUrl;
    }

    private const string BodyFormat =
        @"Thanks for registering with Poker Bunch.

Please sign in here: {0}";
}