using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels
{
    [DataContract(Namespace = "", Name = "player")]
    public class CashgameDetailsPlayerModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "color")]
        public string Color { get; set; }
        [DataMember(Name = "startTime")]
        public DateTime StartTime { get; set; }
        [DataMember(Name = "updatedTime")]
        public DateTime UpdatedTime { get; set; }
        [DataMember(Name = "buyin")]
        public int Buyin { get; set; }
        [DataMember(Name = "stack")]
        public int Stack { get; set; }
        [DataMember(Name = "actions")]
        public IList<CashgameDetailsActionModel> Actions { get; set; }

        public CashgameDetailsPlayerModel(CashgameDetails.RunningCashgamePlayerItem item)
        {
            Id = item.PlayerId.ToString();
            Name = item.Name;
            Color = item.Color;
            StartTime = item.BuyinTime;
            UpdatedTime = item.UpdatedTime;
            Buyin = item.Buyin;
            Stack = item.Stack;
            Actions = item.Checkpoints.Select(o => new CashgameDetailsActionModel(o)).ToList();
        }

        public CashgameDetailsPlayerModel()
        {
        }
    }
}