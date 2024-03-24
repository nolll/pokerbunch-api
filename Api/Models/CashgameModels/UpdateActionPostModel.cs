using System.Text.Json.Serialization;

namespace Api.Models.CashgameModels;

[method: JsonConstructor]
public class UpdateActionPostModel(DateTimeOffset timestamp, int stack, int? added)
{
    public DateTimeOffset Timestamp { get; } = timestamp;
    public int Stack { get; } = stack;
    public int? Added { get; } = added;
}