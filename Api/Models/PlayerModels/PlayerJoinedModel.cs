using Api.Models.CommonModels;

namespace Api.Models.PlayerModels;

public class PlayerJoinedModel : MessageModel
{
    private readonly string _id;
    public override string Message => $"Player joined {_id}";

    public PlayerJoinedModel(string id)
    {
        _id = id;
    }
}