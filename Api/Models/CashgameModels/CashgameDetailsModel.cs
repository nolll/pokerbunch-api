using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Api.Models.LocationModels;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class CashgameDetailsModel
{
    [JsonPropertyName("id")]
    public string Id { get; }
    
    [JsonPropertyName("isRunning")]
    public bool IsRunning { get; }
    
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; }
    
    [JsonPropertyName("updatedTime")]
    public DateTime UpdatedTime { get; }
    
    [JsonPropertyName("bunch")]
    public CashgameBunchModel Bunch { get; }
    
    [JsonPropertyName("location")]
    public SmallLocationModel Location { get; }
    
    [JsonPropertyName("event")]
    public CashgameDetailsEventModel Event { get; }
    
    [JsonPropertyName("players")]
    public IList<CashgameDetailsPlayerModel> Players { get; }

    public CashgameDetailsModel(CashgameDetails.Result details)
    {
        Id = details.CashgameId.ToString();
        IsRunning = details.IsRunning;
        StartTime = details.StartTime;
        UpdatedTime = details.UpdatedTime;
        Bunch = new CashgameBunchModel(details);
        Location = new SmallLocationModel(details);
        Event = details.EventId != 0 ? new CashgameDetailsEventModel(details) : null;
        Players = details.PlayerItems.Select(o => new CashgameDetailsPlayerModel(o)).ToList();
    }

    [JsonConstructor]
    public CashgameDetailsModel(string id, bool isRunning, DateTime startTime, DateTime updatedTime, CashgameBunchModel bunch, SmallLocationModel location, CashgameDetailsEventModel @event, IList<CashgameDetailsPlayerModel> players)
    {
        Id = id;
        IsRunning = isRunning;
        StartTime = startTime;
        UpdatedTime = updatedTime;
        Bunch = bunch;
        Location = location;
        Event = @event;
        Players = players;
    }
}