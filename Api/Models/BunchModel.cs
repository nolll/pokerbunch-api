using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "bunch")]
    public class BunchModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "slug")]
        public string Slug { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "Description")]
        public string Description { get; set; }
        [DataMember(Name = "houseRules")]
        public string HouseRules { get; set; }
        [DataMember(Name = "role")]
        public string Role { get; set; }

        public BunchModel(GetBunch.Result r)
            : this(r.Id, r.Slug, r.Name, r.Description)
        {
            HouseRules = r.HouseRules;
            Role = r.Role.ToString().ToLower();
        }

        public BunchModel(GetBunchList.ResultItem r)
            : this(r.Id, r.Slug, r.Name, r.Description)
        {
        }

        public BunchModel(int id, string slug, string name, string description)
        {
            Id = id;
            Slug = slug;
            Name = name;
            Description = description;
        }

        public BunchModel()
        {
        }
    }
}