using System.Text.Json.Serialization;

namespace Api.Models.CashgameModels;

[method: JsonConstructor]
public class AddCashgamePostModel(string locationId)
{
    public string LocationId { get; } = locationId;
}