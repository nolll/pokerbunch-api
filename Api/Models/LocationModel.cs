using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "location")]
    public class LocationModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "bunch")]
        public string Bunch { get; set; }

        public LocationModel(GetLocationList.Location location)
            : this(location.Id, location.Name, location.Slug)
        {
        }

        public LocationModel(GetLocation.Result location)
            : this(location.Id, location.Name, location.Slug)
        {
        }

        public LocationModel(AddLocation.Result location)
            : this(location.Id, location.Name, location.Slug)
        {
        }

        private LocationModel(int id, string name, string bunch)
        {
            Id = id;
            Name = name;
            Bunch = bunch;
        }

        public LocationModel()
        {
        }
    }
}