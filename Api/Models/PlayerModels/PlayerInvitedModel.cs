using Api.Models.CommonModels;

namespace Api.Models.PlayerModels;

public class PlayerInvitedModel : MessageModel
{
    private readonly string _id;
    public override string Message => $"Player invited {_id}";

    public PlayerInvitedModel(string id)
    {
        _id = id;
    }
}