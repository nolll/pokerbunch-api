using System.Runtime.Serialization;

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
}