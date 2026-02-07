using Api.Models.CommonModels;

namespace Api.Models.JoinRequestModels;

public class JoinRequestSentModel(string bunchName) : MessageModel
{
    public override string Message => $"A join request was sent to {bunchName}";
}