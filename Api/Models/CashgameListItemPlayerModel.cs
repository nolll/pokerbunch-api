using System;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "player")]
    public class CashgameListItemPlayerModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "startTime")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "updatedTime")]
        public DateTime UpdatedTime { get; set; }
        [DataMember(Name = "buyin")]
        public int Buyin { get; set; }
        [DataMember(Name = "stack")]
        public int Stack { get; set; }

        public CashgameListItemPlayerModel(CashgameList.ItemPlayer item)
        {
            Id = item.Id.ToString();
            StartTime = item.BuyinTime;
            UpdatedTime = item.UpdatedTime;
            Buyin = item.Buyin;
            Stack = item.Stack;
        }

        public CashgameListItemPlayerModel()
        {
        }
    }
}