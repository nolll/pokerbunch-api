using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Api.Models.LocationModels;
using Core.UseCases;

namespace Api.Models.CashgameModels
{
    [DataContract(Namespace = "", Name = "cashgame")]
    public class CashgameListItemModel
    {
        [DataMember(Name = "id")]
        public string Id { get; }
        [DataMember(Name = "startTime")]
        public DateTime StartTime { get; }
        [DataMember(Name = "updatedTime")]
        public DateTime UpdatedTime { get; }
        [DataMember(Name = "location")]
        public SmallLocationModel Location { get; }
        [DataMember(Name = "players")]
        public IList<CashgameListItemResultModel> Results { get; }

        public CashgameListItemModel(CashgameList.Item item)
        {
            Id = item.CashgameId.ToString();
            StartTime = item.StartTime;
            UpdatedTime = item.EndTime;
            Location = new SmallLocationModel(item);
            Results = item.Results.Select(o => new CashgameListItemResultModel(o)).ToList();
        }

        public CashgameListItemModel(EventCashgameList.Item item)
        {
            Id = item.CashgameId.ToString();
            StartTime = item.StartTime;
            UpdatedTime = item.EndTime;
            Location = new SmallLocationModel(item);
            Results = item.Results.Select(o => new CashgameListItemResultModel(o)).ToList();
        }

        public CashgameListItemModel(PlayerCashgameList.Item item)
        {
            Id = item.CashgameId.ToString();
            StartTime = item.StartTime;
            UpdatedTime = item.EndTime;
            Location = new SmallLocationModel(item);
            Results = item.Players.Select(o => new CashgameListItemResultModel(o)).ToList();
        }
    }
}