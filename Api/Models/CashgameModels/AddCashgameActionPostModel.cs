using System.Text.Json.Serialization;

namespace Api.Models.CashgameModels;

[method: JsonConstructor]
public class AddCashgameActionPostModel(string type, string playerId, int added, int stack)
{
    public string Type { get; } = type;
    public string PlayerId { get; } = playerId;
    public int Added { get; } = added;
    public int Stack { get; } = stack;
}