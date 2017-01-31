using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "bunch")]
    public class CashgameDetailsBunchModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "timezone")]
        public string Timezone { get; set; }
        [DataMember(Name = "currencyFormat")]
        public string CurrencyFormat { get; set; }
        [DataMember(Name = "currencySymbol")]
        public string CurrencySymbol { get; set; }
        [DataMember(Name = "currencyLayout")]
        public string CurrencyLayout { get; set; }
        [DataMember(Name = "thousandSeparator")]
        public string ThousandSeparator { get; set; }
        [DataMember(Name = "culture")]
        public string Culture { get; set; }
        [DataMember(Name = "role")]
        public string Role { get; set; }

        public CashgameDetailsBunchModel(CashgameDetails.Result details)
        {
            Id = details.Slug;
            Timezone = details.Timezone;
            CurrencyFormat = details.CurrencyFormat;
            CurrencySymbol = details.CurrencySymbol;
            CurrencyLayout = details.CurrencyLayout;
            ThousandSeparator = details.ThousandSeparator;
            Culture = details.Culture;
            Role = details.Role.ToString().ToLower();
        }

        public CashgameDetailsBunchModel()
        {
        }
    }
}