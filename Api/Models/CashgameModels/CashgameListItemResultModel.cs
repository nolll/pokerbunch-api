using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class CashgameListItemResultModel
{
    [JsonPropertyName("id")]
    public string Id { get; }
    
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; }
    
    [JsonPropertyName("updatedTime")]
    public DateTime UpdatedTime { get; }
    
    [JsonPropertyName("buyin")]
    public int Buyin { get; }
    
    [JsonPropertyName("stack")]
    public int Stack { get; }

    public CashgameListItemResultModel(CashgameList.ItemResult item)
    {
        Id = item.Player.Id.ToString();
        Name = item.Player.Name;
        StartTime = item.BuyinTime;
        UpdatedTime = item.UpdatedTime;
        Buyin = item.Buyin;
        Stack = item.Stack;
    }

    public CashgameListItemResultModel(EventCashgameList.ItemResult item)
    {
        Id = item.Player.Id.ToString();
        Name = item.Player.Name;
        StartTime = item.BuyinTime;
        UpdatedTime = item.UpdatedTime;
        Buyin = item.Buyin;
        Stack = item.Stack;
    }

    public CashgameListItemResultModel(PlayerCashgameList.ItemResult item)
    {
        Id = item.Player.Id.ToString();
        Name = item.Player.Name;
        StartTime = item.BuyinTime;
        UpdatedTime = item.UpdatedTime;
        Buyin = item.Buyin;
        Stack = item.Stack;
    }

    [JsonConstructor]
    public CashgameListItemResultModel(string id, string name, DateTime startTime, DateTime updatedTime, int buyin, int stack)
    {
        Id = id;
        Name = name;
        StartTime = startTime;
        UpdatedTime = updatedTime;
        Buyin = buyin;
        Stack = stack;
    }
}