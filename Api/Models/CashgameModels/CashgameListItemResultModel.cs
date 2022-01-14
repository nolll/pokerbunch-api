using System;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels;

[DataContract(Namespace = "", Name = "player")]
public class CashgameListItemResultModel
{
    [DataMember(Name = "id")]
    public string Id { get; }
    [DataMember(Name = "name")]
    public string Name { get; }
    [DataMember(Name = "startTime")]
    public DateTime StartTime { get; }
    [DataMember(Name = "updatedTime")]
    public DateTime UpdatedTime { get; }
    [DataMember(Name = "buyin")]
    public int Buyin { get; }
    [DataMember(Name = "stack")]
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
}