namespace Core.Messages;

public class AcceptJoinRequestMessage(string bunchName) : IMessage
{
    public string Subject => "Poker Bunch Join Request Approved";
    public string Body => $"""
                           Your request to join {bunchName} has been approved.

                           Please sign out and then sign in again to gain access to the bunch.
                           """;
}