using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels;

[method: JsonConstructor]
public class CashgameListItemResultModel(
    string id,
    string name,
    DateTime startTime,
    DateTime updatedTime,
    int buyin,
    int stack)
{
    [JsonPropertyName("id")]
    public string Id { get; } = id;

    [JsonPropertyName("name")]
    public string Name { get; } = name;

    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; } = startTime;

    [JsonPropertyName("updatedTime")]
    public DateTime UpdatedTime { get; } = updatedTime;

    [JsonPropertyName("buyin")]
    public int Buyin { get; } = buyin;

    [JsonPropertyName("stack")]
    public int Stack { get; } = stack;

    public CashgameListItemResultModel(CashgameList.ItemResult item) 
        : this(item.Player.Id, item.Player.Name, item.BuyinTime, item.UpdatedTime, item.Buyin, item.Stack)
    {
    }

    public CashgameListItemResultModel(EventCashgameList.ItemResult item) 
        : this(item.Player.Id, item.Player.Name, item.BuyinTime, item.UpdatedTime, item.Buyin, item.Stack)
    {
    }

    public CashgameListItemResultModel(PlayerCashgameList.ItemResult item) 
        : this(item.Player.Id, item.Player.Name, item.BuyinTime, item.UpdatedTime, item.Buyin, item.Stack)
    {
    }
}