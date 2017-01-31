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
        [DataMember(Name = "buyinTime")]
        public DateTime BuyinTime { get; set; }
        [DataMember(Name = "updatedTime")]
        public DateTime UpdatedTime { get; set; }
        [DataMember(Name = "buyin")]
        public int Buyin { get; set; }
        [DataMember(Name = "cashout")]
        public int Cashout { get; set; }

        public CashgameListItemPlayerModel(CashgameList.ItemPlayer item)
        {
            Id = item.Id.ToString();
            BuyinTime = item.BuyinTime;
            UpdatedTime = item.UpdatedTime;
            Buyin = item.Buyin;
            Cashout = item.Cashout;
        }

        public CashgameListItemPlayerModel()
        {
        }
    }
}