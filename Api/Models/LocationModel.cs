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

        public LocationModel(GetLocationList.Location location)
            : this(location.Id, location.Name)
        {
        }

        public LocationModel(GetLocation.Result location)
            : this(location.Id, location.Name)
        {
        }

        private LocationModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public LocationModel()
        {
        }
    }
}