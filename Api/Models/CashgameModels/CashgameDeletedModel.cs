using Api.Models.CommonModels;

namespace Api.Models.CashgameModels;

public class CashgameDeletedModel : MessageModel
{
    private readonly int _id;
    public override string Message => $"Cashgame deleted {_id}";

    public CashgameDeletedModel(int id)
    {
        _id = id;
    }
}