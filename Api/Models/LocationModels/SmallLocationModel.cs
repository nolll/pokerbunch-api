using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.LocationModels;

[method: JsonConstructor]
public class SmallLocationModel(string? id, string? name)
{
    [JsonPropertyName("id")]
    public string Id { get; } = id ?? "";

    [JsonPropertyName("name")]
    public string Name { get; } = name ?? "";

    public SmallLocationModel(CashgameDetails.Result details)
        : this(details.LocationId, details.LocationName)
    {
    }

    public SmallLocationModel(CashgameList.Item item)
        : this(item.LocationId, item.LocationName)
    {
    }

    public SmallLocationModel(EventCashgameList.Item item)
        : this(item.LocationId, item.LocationName)
    {
    }

    public SmallLocationModel(PlayerCashgameList.Item item)
        : this(item.LocationId, item.LocationName)
    {
    }

    public SmallLocationModel(EventList.Event item)
        : this(item.LocationId, item.LocationName)
    {
    }

    public SmallLocationModel(EventDetails.Result e)
        : this(e.LocationId, e.LocationName)
    {
    }

    public SmallLocationModel(int id, string name)
        : this(id.ToString(), name)
    {
    }
}