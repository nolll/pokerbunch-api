using Api.Models.CommonModels;

namespace Api.Models.CashgameModels;

public class CashgameDeletedModel : MessageModel
{
    private readonly string _id;
    public override string Message => $"Cashgame deleted {_id}";

    public CashgameDeletedModel(string id)
    {
        _id = id;
    }
}