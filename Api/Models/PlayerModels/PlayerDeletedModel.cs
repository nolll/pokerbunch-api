using Api.Models.CommonModels;

namespace Api.Models.PlayerModels;

public class PlayerDeletedModel : MessageModel
{
    private readonly int _id;
    public override string Message => $"Player deleted {_id}";

    public PlayerDeletedModel(int id)
    {
        _id = id;
    }
}