namespace Core.Services;

public class RegistrationMessage(string loginUrl) : IMessage
{
    public string Subject => "Poker Bunch Registration";
    public string Body => string.Format(BodyFormat, loginUrl);

    private const string BodyFormat = """
                                      Thanks for registering with Poker Bunch.

                                      Please sign in here: {0}
                                      """;
}