using System.Runtime.Serialization;
using Api.Extensions;
using Api.Urls.ApiUrls;

namespace Api.Models.CashgameModels;

[DataContract(Namespace = "", Name = "year")]
public class CashgameYearListItemModel
{
    [DataMember(Name = "year")]
    public string Id { get; }
    [DataMember(Name = "url")]
    public string Url { get; }

    public CashgameYearListItemModel(string bunchId, int year, UrlProvider urls)
    {
        Id = year.ToString();
        Url = urls.Api.BunchCashgames(bunchId, year).Absolute();
    }
}