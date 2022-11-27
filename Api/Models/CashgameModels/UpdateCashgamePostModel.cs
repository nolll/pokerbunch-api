using System.Text.Json.Serialization;

namespace Api.Models.CashgameModels;

public class UpdateCashgamePostModel
{
    public string LocationId { get; }
    public string EventId { get; }

    [JsonConstructor]
    public UpdateCashgamePostModel(string locationId, string eventId)
    {
        LocationId = locationId;
        EventId = eventId;
    }
}