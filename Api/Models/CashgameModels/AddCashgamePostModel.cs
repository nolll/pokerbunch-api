using System.Text.Json.Serialization;

namespace Api.Models.CashgameModels;

public class AddCashgamePostModel
{
    public int LocationId { get; }

    [JsonConstructor]
    public AddCashgamePostModel(int locationId)
    {
        LocationId = locationId;
    }
}