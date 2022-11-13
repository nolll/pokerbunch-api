using Api.Models.CommonModels;

namespace Api.Models.PlayerModels;

public class PlayerJoinedModel : MessageModel
{
    private readonly int _id;
    public override string Message => $"Player joined {_id}";

    public PlayerJoinedModel(int id)
    {
        _id = id;
    }
}