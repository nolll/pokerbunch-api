using System.Text.Json.Serialization;

namespace Api.Models.CashgameModels;

public class UpdateCashgamePostModel
{
    public int LocationId { get; }
    public int EventId { get; }

    [JsonConstructor]
    public UpdateCashgamePostModel(int locationId, int eventId)
    {
        LocationId = locationId;
        EventId = eventId;
    }
}