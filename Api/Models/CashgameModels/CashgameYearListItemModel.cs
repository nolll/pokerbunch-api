using System.Runtime.Serialization;
using Api.Extensions;
using PokerBunch.Common.Urls.ApiUrls;

namespace Api.Models.CashgameModels
{
    [DataContract(Namespace = "", Name = "year")]
    public class CashgameYearListItemModel
    {
        [DataMember(Name = "year")]
        public string Id { get; }
        [DataMember(Name = "url")]
        public string Url { get; }

        public CashgameYearListItemModel(string bunchId, int year)
        {
            Id = year.ToString();
            Url = new ApiCashgameListWithYearUrl(bunchId, year).Absolute();
        }
    }
}