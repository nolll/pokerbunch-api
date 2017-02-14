using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "cashgame")]
    public class CashgameListItemModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "startTime")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "endTime")]
        public DateTime EndTime { get; set; }
        [DataMember(Name = "location")]
        public CashgameLocationModel Location { get; set; }
        [DataMember(Name = "players")]
        public IList<CashgameListItemPlayerModel> Players { get; set; }

        public CashgameListItemModel(CashgameList.Item item)
        {
            Id = item.CashgameId.ToString();
            StartTime = item.StartTime;
            EndTime = item.EndTime;
            Location = new CashgameLocationModel(item);
            Players = item.Players.Select(o => new CashgameListItemPlayerModel(o)).ToList();
        }

        public CashgameListItemModel()
        {
        }
    }
}