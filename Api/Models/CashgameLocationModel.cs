using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "location")]
    public class CashgameLocationModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }

        public CashgameLocationModel(CashgameDetails.Result details)
            : this(details.LocationId, details.LocationName)
        {
        }

        public CashgameLocationModel(CashgameList.Item item)
            : this(item.LocationId, item.LocationName)
        {
        }

        public CashgameLocationModel(int id, string name)
            : this(id.ToString(), name)
        {
        }

        public CashgameLocationModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public CashgameLocationModel()
        {
        }
    }
}