using System.Text.Json.Serialization;

namespace Api.Models.CashgameModels;

[method: JsonConstructor]
public class UpdateCashgamePostModel(string locationId, string? eventId)
{
    public string LocationId { get; } = locationId;
    public string? EventId { get; } = eventId;
}