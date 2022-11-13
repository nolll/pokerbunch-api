using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class CashgameDetailsEventModel
{
    [JsonPropertyName("id")]
    public string Id { get; }
    
    [JsonPropertyName("name")]
    public string Name { get; }

    public CashgameDetailsEventModel(CashgameDetails.Result details)
    {
        Id = details.EventId.ToString();
        Name = details.EventName;
    }

    [JsonConstructor]
    public CashgameDetailsEventModel(string id, string name)
    {
        Id = id;
        Name = name;
    }
}