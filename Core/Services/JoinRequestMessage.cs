namespace Core.Services;

public class JoinRequestMessage(string bunchName, string userName, string url) : IMessage
{
    public string Subject => "Poker Bunch Join Request";
    public string Body => string.Format(BodyFormat, bunchName, userName, url);

    private const string BodyFormat = """
                                      The user {1} wants to join {0}.

                                      You can accept or deny the request here: {2}
                                      """;
}