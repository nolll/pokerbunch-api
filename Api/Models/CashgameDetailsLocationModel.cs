using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "location")]
    public class CashgameDetailsLocationModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }

        public CashgameDetailsLocationModel(CashgameDetails.Result details)
        {
            Id = details.LocationId.ToString();
            Name = details.LocationName;
        }

        public CashgameDetailsLocationModel()
        {
        }
    }
}