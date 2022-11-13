using System;
using System.Text.Json.Serialization;

namespace Api.Models.CashgameModels;

public class UpdateActionPostModel
{
    public DateTimeOffset Timestamp { get; }
    public int Stack { get; }
    public int? Added { get; }

    [JsonConstructor]
    public UpdateActionPostModel(DateTimeOffset timestamp, int stack, int? added)
    {
        Timestamp = timestamp;
        Stack = stack;
        Added = added;
    }
}