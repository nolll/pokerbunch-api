using Api.Models.CommonModels;

namespace Api.Models.PlayerModels;

public class PlayerInvitedModel(string id) : MessageModel
{
    public override string Message => $"Player invited {id}";
}