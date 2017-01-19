using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.Entities.Checkpoints;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "player")]
    public class ApiCashgameTopListItem
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "winnings")]
        public int Winnings { get; set; }

        public ApiCashgameTopListItem(string name, int winnings)
        {
            Name = name;
            Winnings = winnings;
        }

        public ApiCashgameTopListItem()
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
        [DataMember(Name = "endTime")]
        public DateTime EndTime { get; set; }
        [DataMember(Name = "players")]
        public IList<CashgameListItemPlayerModel> Players { get; set; }

        public CashgameListItemModel(CashgameList.Item item)
        {
            Id = item.CashgameId.ToString();
            StartTime = item.StartTime;
            EndTime = item.EndTime;
            Players = item.Players.Select(o => new CashgameListItemPlayerModel(o)).ToList();
        }

        public CashgameListItemModel()
        {
        }
    }

    [DataContract(Namespace = "", Name = "player")]
    public class CashgameListItemPlayerModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "buyinTime")]
        public DateTime BuyinTime { get; set; }
        [DataMember(Name = "lastActionTime")]
        public DateTime LastActionTime { get; set; }
        [DataMember(Name = "buyin")]
        public int Buyin { get; set; }
        [DataMember(Name = "cashout")]
        public int Cashout { get; set; }

        public CashgameListItemPlayerModel(CashgameList.ItemPlayer item)
        {
            Id = item.Id.ToString();
            BuyinTime = item.BuyinTime;
            LastActionTime = item.LastActionTime;
            Buyin = item.Buyin;
            Cashout = item.Cashout;
        }

        public CashgameListItemPlayerModel()
        {
        }
    }

    [DataContract(Namespace = "", Name = "cashgame")]
    public class CashgameDetailsModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "location")]
        public string Location { get; set; }
        [DataMember(Name = "isRunning")]
        public bool IsRunning { get; set; }
        [DataMember(Name = "startTime")]
        public DateTime? StartTime { get; set; }
        [DataMember(Name = "endTime")]
        public DateTime? EndTime { get; set; }
        [DataMember(Name = "players")]
        public IList<CashgameDetailsPlayerModel> Players { get; set; }

        public CashgameDetailsModel(CashgameDetails.Result details)
        {
            Id = details.CashgameId.ToString();
            Location = details.LocationName;
            IsRunning = details.IsRunning;
            Players = details.PlayerItems.Select(o => new CashgameDetailsPlayerModel(o)).ToList();
        }

        public CashgameDetailsModel()
        {
        }
    }

    [DataContract(Namespace = "", Name = "player")]
    public class CashgameDetailsPlayerModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "color")]
        public string Color { get; set; }
        [DataMember(Name = "buyinTime")]
        public DateTime BuyinTime { get; set; }
        [DataMember(Name = "lastActionTime")]
        public DateTime LastActionTime { get; set; }
        [DataMember(Name = "buyin")]
        public int Buyin { get; set; }
        [DataMember(Name = "stack")]
        public int Stack { get; set; }
        [DataMember(Name = "actions")]
        public IList<CashgameDetailsCheckpointModel> Actions { get; set; }

        public CashgameDetailsPlayerModel(CashgameDetails.RunningCashgamePlayerItem item)
        {
            Id = item.PlayerId.ToString();
            Name = item.Name;
            Color = item.Color;
            BuyinTime = item.BuyinTime;
            LastActionTime = item.LastActionTime;
            Buyin = item.Buyin;
            Stack = item.Stack;
            Actions = item.Checkpoints.Select(o => new CashgameDetailsCheckpointModel(o)).ToList();
        }

        public CashgameDetailsPlayerModel()
        {
        }
    }

    [DataContract(Namespace = "", Name = "actions")]
    public class CashgameDetailsCheckpointModel
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "time")]
        public DateTime Time { get; set; }
        [DataMember(Name = "stack")]
        public int Stack { get; set; }
        [DataMember(Name = "added")]
        public int? Added { get; set; }

        public CashgameDetailsCheckpointModel(CashgameDetails.RunningCashgameCheckpointItem item)
        {
            Type = item.Type.ToString().ToLower();
            Time = item.Time;
            Stack = item.Stack;

            if (item.Type == CheckpointType.Buyin)
            {
                Added = item.AddedMoney;
            }
        }

        public CashgameDetailsCheckpointModel()
        {
        }
    }
}