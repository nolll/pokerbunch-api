using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Api.Models.LocationModels;
using Core.UseCases;

namespace Api.Models.CashgameModels;

[DataContract(Namespace = "", Name = "cashgame")]
public class CashgameDetailsModel
{
    [DataMember(Name = "id")]
    public string Id { get; }
    [DataMember(Name = "isRunning")]
    public bool IsRunning { get; }
    [DataMember(Name = "startTime")]
    public DateTime StartTime { get; }
    [DataMember(Name = "updatedTime")]
    public DateTime UpdatedTime { get; }
    [DataMember(Name = "bunch")]
    public CashgameBunchModel Bunch { get; }
    [DataMember(Name = "location")]
    public SmallLocationModel Location { get; }
    [DataMember(Name = "event")]
    public CashgameDetailsEventModel Event { get; }
    [DataMember(Name = "players")]
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
}