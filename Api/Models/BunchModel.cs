using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "bunch")]
    public class BunchModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "Description")]
        public string Description { get; set; }
        [DataMember(Name = "houseRules")]
        public string HouseRules { get; set; }
        [DataMember(Name = "timezone")]
        public string Timezone { get; set; }
        [DataMember(Name = "currencySymbol")]
        public string CurrencySymbol { get; set; }
        [DataMember(Name = "currencyLayout")]
        public string CurrencyLayout { get; set; }
        [DataMember(Name = "defaultBuyin")]
        public int DefaultBuyin { get; set; }
        [DataMember(Name = "role")]
        public string Role { get; set; }

        public BunchModel(GetBunch.Result r)
            : this(r.Slug, r.Name, r.Description)
        {
            HouseRules = r.HouseRules;
            Timezone = r.Timezone.Id;
            CurrencySymbol = r.Currency.Symbol;
            CurrencyLayout = r.Currency.Layout;
            DefaultBuyin = r.DefaultBuyin;
            Role = r.Role.ToString().ToLower();
        }

        public BunchModel(GetBunchList.ResultItem r)
            : this(r.Slug, r.Name, r.Description)
        {
        }

        public BunchModel(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public BunchModel()
        {
        }
    }
}