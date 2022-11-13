using System;
using System.Text.Json.Serialization;
using Core.Entities.Checkpoints;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class CashgameDetailsActionModel
{
    [JsonPropertyName("id")]
    public string Id { get; }
    [JsonPropertyName("type")]
    public string Type { get; }
    [JsonPropertyName("time")]
    public DateTime Time { get; }
    [JsonPropertyName("stack")]
    public int Stack { get; }
    [JsonPropertyName("added")]
    public int? Added { get; }

    public CashgameDetailsActionModel(CashgameDetails.RunningCashgameActionItem item)
    {
        Id = item.Id;
        Type = item.Type.ToString().ToLower();
        Time = item.Time;
        Stack = item.Stack;

        if (item.Type == CheckpointType.Buyin)
        {
            Added = item.AddedMoney;
        }
    }

    [JsonConstructor]
    public CashgameDetailsActionModel(string id, string type, DateTime time, int stack, int? added)
    {
        Id = id;
        Type = type;
        Time = time;
        Stack = stack;
        Added = added;
    }
}