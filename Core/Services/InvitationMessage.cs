namespace Core.Services;

public class InvitationMessage(
    string bunchName,
    string invitationCode,
    string registerUrl,
    string joinUrl,
    string joinWithCodeUrl)
    : IMessage
{
    public string Subject => $"Invitation to Poker Bunch: {bunchName}";
    public string Body => string.Format(BodyFormat, bunchName, joinUrl, invitationCode, registerUrl, joinWithCodeUrl);

    private const string BodyFormat = """
                                      You have been invited to join the poker game: {0}.

                                      Use this link to accept the invitation: {4}. If the link doesn't work in your email client,
                                      use this link instead, {1}, and enter this verification code: {2}

                                      If you don't have an account, you can register at {3}
                                      """;
}