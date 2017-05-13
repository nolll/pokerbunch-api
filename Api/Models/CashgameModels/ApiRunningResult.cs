using System;
using System.Runtime.Serialization;
using Api.Urls.ApiUrls;
using Core.UseCases;

namespace Api.Models.CashgameModels
{
    [DataContract(Namespace = "", Name = "result")]
    public class ApiRunningResult
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "buyin")]
        public int Buyin { get; set; }
        [DataMember(Name = "stack")]
        public int Stack { get; set; }
        [DataMember(Name = "winnings")]
        public int Winnings { get; set; }
        [DataMember(Name = "lastupdate")]
        public DateTime LastUpdate { get; set; }

        public ApiRunningResult(CashgameDetails.RunningCashgamePlayerItem playerItem)
        {
            Name = playerItem.Name;
            Buyin = playerItem.Buyin;
            Stack = playerItem.Stack;
            Winnings = playerItem.Winnings;
            LastUpdate = playerItem.UpdatedTime;
        }
    }

    [DataContract(Namespace = "", Name = "game")]
    public class ApiCurrentGame
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }

        public ApiCurrentGame(CurrentCashgames.Game game)
        {
            Id = game.Id.ToString();
            Url = new ApiCashgameUrl(game.Id).Absolute;
        }
    }
}