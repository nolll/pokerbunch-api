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
        [DataMember(Name = "canEdit")]
        public bool CanEdit { get; set; }

        public BunchModel(GetBunch.Result getBunchResult)
        {
            Id = getBunchResult.Id;
            Slug = getBunchResult.Slug;
            Name = getBunchResult.BunchName;
            Description = getBunchResult.Description;
            HouseRules = getBunchResult.HouseRules;
            CanEdit = getBunchResult.CanEdit;
        }

        public BunchModel(int id, string slug, string name)
        {
            Id = id;
            Slug = slug;
            Name = name;
        }

        public BunchModel()
        {
        }
    }
}