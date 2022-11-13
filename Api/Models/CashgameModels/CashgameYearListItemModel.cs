using System.Text.Json.Serialization;
using Api.Urls.ApiUrls;

namespace Api.Models.CashgameModels;

public class CashgameYearListItemModel
{
    [JsonPropertyName("year")]
    public string Id { get; }
    
    [JsonPropertyName("url")]
    public string Url { get; }

    public CashgameYearListItemModel(string bunchId, int year, UrlProvider urls)
    {
        Id = year.ToString();
        Url = urls.Api.BunchCashgames(bunchId, year);
    }

    [JsonConstructor]
    public CashgameYearListItemModel(string id, string url)
    {
        Id = id;
        Url = url;
    }
}