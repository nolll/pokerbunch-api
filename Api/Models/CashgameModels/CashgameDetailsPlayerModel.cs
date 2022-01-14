using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels;

[DataContract(Namespace = "", Name = "player")]
public class CashgameDetailsPlayerModel
{
    [DataMember(Name = "id")]
    public string Id { get; }
    [DataMember(Name = "name")]
    public string Name { get; }
    [DataMember(Name = "color")]
    public string Color { get; }
    [DataMember(Name = "startTime")]
    public DateTime StartTime { get; }
    [DataMember(Name = "updatedTime")]
    public DateTime UpdatedTime { get; }
    [DataMember(Name = "buyin")]
    public int Buyin { get; }
    [DataMember(Name = "stack")]
    public int Stack { get; }
    [DataMember(Name = "actions")]
    public IList<CashgameDetailsActionModel> Actions { get; }

    public CashgameDetailsPlayerModel(CashgameDetails.RunningCashgamePlayerItem item)
    {
        Id = item.PlayerId.ToString();
        Name = item.Name;
        Color = item.Color;
        StartTime = item.BuyinTime;
        UpdatedTime = item.UpdatedTime;
        Buyin = item.Buyin;
        Stack = item.Stack;
        Actions = item.Checkpoints.Select(o => new CashgameDetailsActionModel(o)).ToList();
    }
}