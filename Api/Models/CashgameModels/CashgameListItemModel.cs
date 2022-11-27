using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Api.Models.LocationModels;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class CashgameListItemModel
{
    [JsonPropertyName("id")]
    public string Id { get; }
    
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; }
    
    [JsonPropertyName("updatedTime")]
    public DateTime UpdatedTime { get; }
    
    [JsonPropertyName("location")]
    public SmallLocationModel Location { get; }
    
    [JsonPropertyName("results")]
    public IList<CashgameListItemResultModel> Results { get; }

    public CashgameListItemModel(CashgameList.Item item)
    {
        Id = item.CashgameId;
        StartTime = item.StartTime;
        UpdatedTime = item.EndTime;
        Location = new SmallLocationModel(item);
        Results = item.Results.Select(o => new CashgameListItemResultModel(o)).ToList();
    }

    public CashgameListItemModel(EventCashgameList.Item item)
    {
        Id = item.CashgameId;
        StartTime = item.StartTime;
        UpdatedTime = item.EndTime;
        Location = new SmallLocationModel(item);
        Results = item.Results.Select(o => new CashgameListItemResultModel(o)).ToList();
    }

    public CashgameListItemModel(PlayerCashgameList.Item item)
    {
        Id = item.CashgameId;
        StartTime = item.StartTime;
        UpdatedTime = item.EndTime;
        Location = new SmallLocationModel(item);
        Results = item.Players.Select(o => new CashgameListItemResultModel(o)).ToList();
    }

    [JsonConstructor]
    public CashgameListItemModel(string id, DateTime startTime, DateTime updatedTime, SmallLocationModel location, IList<CashgameListItemResultModel> results)
    {
        Id = id;
        StartTime = startTime;
        UpdatedTime = updatedTime;
        Location = location;
        Results = results;
    }
}