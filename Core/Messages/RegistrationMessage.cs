namespace Core.Messages;

public class RegistrationMessage(string loginUrl) : IMessage
{
    public string Subject => "Poker Bunch Registration";
    public string Body => $"""
                           Thanks for registering with Poker Bunch.

                           Please sign in here: {loginUrl}
                           """;
}