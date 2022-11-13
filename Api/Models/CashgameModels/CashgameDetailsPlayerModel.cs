using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class CashgameDetailsPlayerModel
{
    [JsonPropertyName("id")]
    public string Id { get; }
    
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("color")]
    public string Color { get; }
    
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; }
    
    [JsonPropertyName("updatedTime")]
    public DateTime UpdatedTime { get; }
    
    [JsonPropertyName("buyin")]
    public int Buyin { get; }
    
    [JsonPropertyName("stack")]
    public int Stack { get; }
    
    [JsonPropertyName("actions")]
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

    [JsonConstructor]
    public CashgameDetailsPlayerModel(string id, string name, string color, DateTime startTime, DateTime updatedTime, int buyin, int stack, IList<CashgameDetailsActionModel> actions)
    {
        Id = id;
        Name = name;
        Color = color;
        StartTime = startTime;
        UpdatedTime = updatedTime;
        Buyin = buyin;
        Stack = stack;
        Actions = actions;
    }
}