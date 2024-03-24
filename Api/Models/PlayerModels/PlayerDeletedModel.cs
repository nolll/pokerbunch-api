using Api.Models.CommonModels;

namespace Api.Models.PlayerModels;

public class PlayerDeletedModel(string id) : MessageModel
{
    public override string Message => $"Player deleted {id}";
}