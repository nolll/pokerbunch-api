namespace Core.Services;

public class InvitationMessage : IMessage
{
    private readonly string _bunchName;
    private readonly string _invitationCode;
    private readonly string _registerUrl;
    private readonly string _joinUrl;
    private readonly string _joinWithCodeUrl;

    public InvitationMessage(string bunchName, string invitationCode, string registerUrl, string joinUrl, string joinWithCodeUrl)
    {
        _bunchName = bunchName;
        _invitationCode = invitationCode;
        _registerUrl = registerUrl;
        _joinUrl = joinUrl;
        _joinWithCodeUrl = joinWithCodeUrl;
    }

    public string Subject => $"Invitation to Poker Bunch: {_bunchName}";
    public string Body => string.Format(BodyFormat, _bunchName, _joinUrl, _invitationCode, _registerUrl, _joinWithCodeUrl);

    private const string BodyFormat =
        @"You have been invited to join the poker game: {0}.

Use this link to accept the invitation: {4}. If the link doesn't work in your email client,
use this link instead, {1}, and enter this verification code: {2}

If you don't have an account, you can register at {3}";
}