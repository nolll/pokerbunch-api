using Api.Models.CommonModels;

namespace Api.Models.PlayerModels;

public class PlayerJoinedModel(string id) : MessageModel
{
    public override string Message => $"Player joined {id}";
}