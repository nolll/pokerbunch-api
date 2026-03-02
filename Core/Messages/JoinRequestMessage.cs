namespace Core.Messages;

public class JoinRequestMessage(string bunchName, string userName, string url) : IMessage
{
    public string Subject => "Poker Bunch Join Request";
    public string Body => $"""
                           The user {userName} wants to join {bunchName}.

                           You can accept or deny the request here: {url}
                           """;
}