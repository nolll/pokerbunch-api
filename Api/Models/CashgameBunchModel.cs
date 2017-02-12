using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "bunch")]
    public class CashgameBunchModel
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

        public CashgameBunchModel(CashgameList.Result listResult)
        {
            Id = listResult.Slug;
            Timezone = listResult.Timezone;
            CurrencyFormat = listResult.CurrencyFormat;
            CurrencySymbol = listResult.CurrencySymbol;
            CurrencyLayout = listResult.CurrencyLayout;
            ThousandSeparator = listResult.ThousandSeparator;
            Culture = listResult.Culture;
            Role = listResult.Role.ToString().ToLower();
        }

        public CashgameBunchModel()
        {
        }
    }
}