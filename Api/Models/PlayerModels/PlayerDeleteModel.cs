using Api.Models.CommonModels;

namespace Api.Models.PlayerModels;

public class PlayerDeleteModel : MessageModel
{
    private readonly int _id;
    public override string Message => $"Player deleted {_id}";

    public PlayerDeleteModel(int id)
    {
        _id = id;
    }
}