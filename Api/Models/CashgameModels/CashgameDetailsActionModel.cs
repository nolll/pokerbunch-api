using System;
using System.Runtime.Serialization;
using Core.Entities.Checkpoints;
using Core.UseCases;

namespace Api.Models.CashgameModels
{
    [DataContract(Namespace = "", Name = "actions")]
    public class CashgameDetailsActionModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "time")]
        public DateTime Time { get; set; }
        [DataMember(Name = "stack")]
        public int Stack { get; set; }
        [DataMember(Name = "added")]
        public int? Added { get; set; }

        public CashgameDetailsActionModel(CashgameDetails.RunningCashgameActionItem item)
        {
            Id = item.Id;
            Type = item.Type.ToString().ToLower();
            Time = item.Time;
            Stack = item.Stack;

            if (item.Type == CheckpointType.Buyin)
            {
                Added = item.AddedMoney;
            }
        }

        public CashgameDetailsActionModel()
        {
        }
    }
}