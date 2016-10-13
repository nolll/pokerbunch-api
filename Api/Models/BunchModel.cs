using System.Runtime.Serialization;
using Core.Entities;
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

        public BunchModel(GetBunch.Result result)
        {
            Id = result.Id;
            Slug = result.Slug;
            Name = result.BunchName;
            Description = result.Description;
            HouseRules = result.HouseRules;
            Role = result.Role.ToString().ToLower();
        }

        public BunchModel(GetBunchList.ResultItem result)
        {
            Id = result.Id;
            Slug = result.Slug;
            Name = result.DisplayName;
            Description = result.Description;
        }

        public BunchModel()
        {
        }
    }
}