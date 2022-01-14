using System.Runtime.Serialization;
using Api.Models.CommonModels;

namespace Api.Models.PlayerModels;

[DataContract(Namespace = "", Name = "player")]
public class PlayerInvitedModel : MessageModel
{
    private readonly int _id;
    public override string Message => $"Player invited {_id}";

    public PlayerInvitedModel(int id)
    {
        _id = id;
    }
}