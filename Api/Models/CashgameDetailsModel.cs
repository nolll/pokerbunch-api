using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "cashgame")]
    public class CashgameDetailsModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "isRunning")]
        public bool IsRunning { get; set; }
        [DataMember(Name = "startTime")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "updatedTime")]
        public DateTime UpdatedTime { get; set; }
        [DataMember(Name = "bunch")]
        public CashgameBunchModel Bunch { get; set; }
        [DataMember(Name = "location")]
        public SmallLocationModel Location { get; set; }
        [DataMember(Name = "event")]
        public CashgameDetailsEventModel Event { get; set; }
        [DataMember(Name = "players")]
        public IList<CashgameDetailsPlayerModel> Players { get; set; }

        public CashgameDetailsModel(CashgameDetails.Result details)
        {
            Id = details.CashgameId.ToString();
            IsRunning = details.IsRunning;
            StartTime = details.StartTime;
            UpdatedTime = details.UpdatedTime;
            Bunch = new CashgameBunchModel(details);
            Location = new SmallLocationModel(details);
            Event = details.EventId != 0 ? new CashgameDetailsEventModel(details) : null;
            Players = details.PlayerItems.Select(o => new CashgameDetailsPlayerModel(o)).ToList();
        }

        public CashgameDetailsModel()
        {
        }
    }
}