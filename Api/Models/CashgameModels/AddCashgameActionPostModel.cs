using System.Text.Json.Serialization;

namespace Api.Models.CashgameModels;

public class AddCashgameActionPostModel
{
    public string Type { get; }
    public string PlayerId { get; }
    public int Added { get; }
    public int Stack { get; }

    [JsonConstructor]
    public AddCashgameActionPostModel(string type, string playerId, int added, int stack)
    {
        Type = type;
        PlayerId = playerId;
        Added = added;
        Stack = stack;
    }
}