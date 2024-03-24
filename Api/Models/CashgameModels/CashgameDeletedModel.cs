using Api.Models.CommonModels;

namespace Api.Models.CashgameModels;

public class CashgameDeletedModel(string id) : MessageModel
{
    public override string Message => $"Cashgame deleted {id}";
}