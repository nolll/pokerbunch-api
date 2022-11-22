using Api.Models.CommonModels;

namespace Api.Models.PlayerModels;

public class PlayerDeletedModel : MessageModel
{
    private readonly string _id;
    public override string Message => $"Player deleted {_id}";

    public PlayerDeletedModel(string id)
    {
        _id = id;
    }
}