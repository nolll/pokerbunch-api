using System.Runtime.Serialization;
using Api.Models.CommonModels;

namespace Api.Models.PlayerModels;

[DataContract(Namespace = "", Name = "player")]
public class PlayerDeleteModel : MessageModel
{
    private readonly int _id;
    public override string Message => $"Player deleted {_id}";

    public PlayerDeleteModel(int id)
    {
        _id = id;
    }
}