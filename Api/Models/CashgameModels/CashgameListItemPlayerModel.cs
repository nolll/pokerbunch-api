using System;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels
{
    [DataContract(Namespace = "", Name = "player")]
    public class CashgameListItemPlayerModel
    {
        [DataMember(Name = "id")]
        public string Id { get; }
        [DataMember(Name = "startTime")]
        public DateTime StartTime { get; }
        [DataMember(Name = "updatedTime")]
        public DateTime UpdatedTime { get; }
        [DataMember(Name = "buyin")]
        public int Buyin { get; }
        [DataMember(Name = "stack")]
        public int Stack { get; }

        public CashgameListItemPlayerModel(CashgameList.ItemPlayer item)
        {
            Id = item.Id.ToString();
            StartTime = item.BuyinTime;
            UpdatedTime = item.UpdatedTime;
            Buyin = item.Buyin;
            Stack = item.Stack;
        }

        public CashgameListItemPlayerModel(EventCashgameList.ItemPlayer item)
        {
            Id = item.Id.ToString();
            StartTime = item.BuyinTime;
            UpdatedTime = item.UpdatedTime;
            Buyin = item.Buyin;
            Stack = item.Stack;
        }
    }
}