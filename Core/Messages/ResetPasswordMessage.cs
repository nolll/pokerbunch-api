namespace Core.Messages;

public class ResetPasswordMessage(string password, string loginUrl) : IMessage
{
    public string Subject => "Poker Bunch Password Recovery";
    public string Body => $"""
                           Here is your new password for Poker Bunch:
                           {password}

                           Please sign in here: {loginUrl}
                           """;
}