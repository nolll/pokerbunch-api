using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.BunchModels
{
    [DataContract(Namespace = "", Name = "bunch")]
    public class BunchModel
    {
        [DataMember(Name = "id")]
        public string Id { get; }

        [DataMember(Name = "name")]
        public string Name { get; }

        [DataMember(Name = "description")]
        public string Description { get; }

        [DataMember(Name = "houseRules")]
        public string HouseRules { get; }

        [DataMember(Name = "timezone")]
        public string Timezone { get; }

        [DataMember(Name = "currencySymbol")]
        public string CurrencySymbol { get; }

        [DataMember(Name = "currencyLayout")]
        public string CurrencyLayout { get; }

        [DataMember(Name = "currencyFormat")]
        public string CurrencyFormat { get; }

        [DataMember(Name = "thousandSeparator")]
        public string ThousandSeparator { get; }

        [DataMember(Name = "defaultBuyin")]
        public int DefaultBuyin { get; }

        [DataMember(Name = "player")]
        public BunchPlayerModel Player { get; }

        [DataMember(Name = "role")]
        public string Role { get; }

        public BunchModel(BunchResult r)
            : this(r.Slug, r.Name, r.Description)
        {
            HouseRules = r.HouseRules;
            Timezone = r.Timezone.Id;
            CurrencySymbol = r.Currency.Symbol;
            CurrencyLayout = r.Currency.Layout;
            CurrencyFormat = r.Currency.Format;
            ThousandSeparator = r.Currency.ThousandSeparator;
            DefaultBuyin = r.DefaultBuyin;
            Player = r.Player != null ? new BunchPlayerModel(r.Player?.Id.ToString(), r.Player?.Name) : null;
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

        public class BunchPlayerModel
        {
            [DataMember(Name = "id")]
            public string Id { get; }
            [DataMember(Name = "name")]
            public string Name { get; }

            public BunchPlayerModel(string id, string name)
            {
                Id = id;
                Name = name;
            }
        }
    }
}