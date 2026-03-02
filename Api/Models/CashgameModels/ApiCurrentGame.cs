using System.Text.Json.Serialization;
using Core.Services.Interfaces;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class ApiCurrentGame
{
    [JsonPropertyName("id")]
    public string Id { get; }
    
    [JsonPropertyName("url")]
    public string Url { get; }
    
    public ApiCurrentGame(CurrentCashgames.Game game, IApiUrlProvider urls)
    {
        Id = game.Id;
        Url = urls.Cashgame(game.Id);
    }

    [JsonConstructor]
    public ApiCurrentGame(string id, string url)
    {
        Id = id;
        Url = url;
    }
}