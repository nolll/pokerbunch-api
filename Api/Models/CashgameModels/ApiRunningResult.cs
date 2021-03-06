using System;
using System.Runtime.Serialization;
using Api.Extensions;
using Api.Urls.ApiUrls;
using Core.UseCases;

namespace Api.Models.CashgameModels;

[DataContract(Namespace = "", Name = "result")]
public class ApiRunningResult
{
    [DataMember(Name = "name")]
    public string Name { get; }
    [DataMember(Name = "buyin")]
    public int Buyin { get; }
    [DataMember(Name = "stack")]
    public int Stack { get; }
    [DataMember(Name = "winnings")]
    public int Winnings { get; }
    [DataMember(Name = "lastupdate")]
    public DateTime LastUpdate { get; }

    public ApiRunningResult(CashgameDetails.RunningCashgamePlayerItem playerItem)
    {
        Name = playerItem.Name;
        Buyin = playerItem.Buyin;
        Stack = playerItem.Stack;
        Winnings = playerItem.Winnings;
        LastUpdate = playerItem.UpdatedTime;
    }
}

[DataContract(Namespace = "", Name = "game")]
public class ApiCurrentGame
{
    [DataMember(Name = "id")]
    public string Id { get; }
    [DataMember(Name = "url")]
    public string Url { get; }

    public ApiCurrentGame(CurrentCashgames.Game game, UrlProvider urls)
    {
        Id = game.Id.ToString();
        Url = urls.Api.Cashgame(game.Id.ToString()).Absolute();
    }
}