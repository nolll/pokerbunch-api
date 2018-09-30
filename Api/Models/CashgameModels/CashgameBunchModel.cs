using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels
{
    [DataContract(Namespace = "", Name = "bunch")]
    public class CashgameBunchModel
    {
        [DataMember(Name = "id")]
        public string Id { get; }
        [DataMember(Name = "timezone")]
        public string Timezone { get; }
        [DataMember(Name = "currencyFormat")]
        public string CurrencyFormat { get; }
        [DataMember(Name = "currencySymbol")]
        public string CurrencySymbol { get; }
        [DataMember(Name = "currencyLayout")]
        public string CurrencyLayout { get; }
        [DataMember(Name = "thousandSeparator")]
        public string ThousandSeparator { get; }
        [DataMember(Name = "culture")]
        public string Culture { get; }
        [DataMember(Name = "role")]
        public string Role { get; }

        public CashgameBunchModel(CashgameDetails.Result detailsResult)
        {
            Id = detailsResult.Slug;
            Timezone = detailsResult.Timezone;
            CurrencyFormat = detailsResult.CurrencyFormat;
            CurrencySymbol = detailsResult.CurrencySymbol;
            CurrencyLayout = detailsResult.CurrencyLayout;
            ThousandSeparator = detailsResult.ThousandSeparator;
            Culture = detailsResult.Culture;
            Role = detailsResult.Role.ToString().ToLower();
        }
    }
}