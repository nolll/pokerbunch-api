using Api.Models.CommonModels;

namespace Api.Models.CashgameModels;

public class CashgameDeleteModel : MessageModel
{
    private readonly int _id;
    public override string Message => $"Cashgame deleted {_id}";

    public CashgameDeleteModel(int id)
    {
        _id = id;
    }
}