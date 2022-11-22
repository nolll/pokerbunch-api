using System.Text.Json.Serialization;

namespace Api.Models.CashgameModels;

public class AddCashgamePostModel
{
    public string LocationId { get; }

    [JsonConstructor]
    public AddCashgamePostModel(string locationId)
    {
        LocationId = locationId;
    }
}