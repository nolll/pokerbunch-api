using System;
using System.Text.Json.Serialization;
using Api.Urls.ApiUrls;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class ApiRunningResult
{
    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("buyin")]
    public int Buyin { get; }
    
    [JsonPropertyName("stack")]
    public int Stack { get; }
    
    [JsonPropertyName("winnings")]
    public int Winnings { get; }
    
    [JsonPropertyName("lastupdate")]
    public DateTime LastUpdate { get; }

    public ApiRunningResult(CashgameDetails.RunningCashgamePlayerItem playerItem)
    {
        Name = playerItem.Name;
        Buyin = playerItem.Buyin;
        Stack = playerItem.Stack;
        Winnings = playerItem.Winnings;
        LastUpdate = playerItem.UpdatedTime;
    }

    [JsonConstructor]
    public ApiRunningResult(string name, int buyin, int stack, int winnings, DateTime lastUpdate)
    {
        Name = name;
        Buyin = buyin;
        Stack = stack;
        Winnings = winnings;
        LastUpdate = lastUpdate;
    }
}

public class ApiCurrentGame
{
    [JsonPropertyName("id")]
    public string Id { get; }
    
    [JsonPropertyName("url")]
    public string Url { get; }

    public ApiCurrentGame(CurrentCashgames.Game game, UrlProvider urls)
    {
        Id = game.Id.ToString();
        Url = urls.Api.Cashgame(game.Id.ToString());
    }

    [JsonConstructor]
    public ApiCurrentGame(string id, string url)
    {
        Id = id;
        Url = url;
    }
}