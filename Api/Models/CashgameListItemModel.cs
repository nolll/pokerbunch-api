using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Api.Extensions;
using Api.Urls;
using Api.Urls.ApiUrls;
using Api.Urls.SiteUrls;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "year")]
    public class CashgameYearListItemModel
    {
        [DataMember(Name = "year")]
        public string Id { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }

        public CashgameYearListItemModel(string bunchId, int year)
        {
            Id = year.ToString();
            Url = new ApiCashgameListUrl(bunchId, year).GetAbsolute();
        }

        public CashgameYearListItemModel()
        {
        }
    }

    [DataContract(Namespace = "", Name = "cashgame")]
    public class CashgameListItemModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "startTime")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "updatedTime")]
        public DateTime UpdatedTime { get; set; }
        [DataMember(Name = "location")]
        public SmallLocationModel Location { get; set; }
        [DataMember(Name = "players")]
        public IList<CashgameListItemPlayerModel> Players { get; set; }

        public CashgameListItemModel(CashgameList.Item item)
        {
            Id = item.CashgameId.ToString();
            StartTime = item.StartTime;
            UpdatedTime = item.EndTime;
            Location = new SmallLocationModel(item);
            Players = item.Players.Select(o => new CashgameListItemPlayerModel(o)).ToList();
        }

        public CashgameListItemModel()
        {
        }
    }
}