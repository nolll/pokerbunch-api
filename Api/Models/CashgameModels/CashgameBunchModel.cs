using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels;

public class CashgameBunchModel
{
    [JsonPropertyName("id")]
    public string Id { get; }
    [JsonPropertyName("timezone")]
    public string Timezone { get; }
    [JsonPropertyName("currencyFormat")]
    public string CurrencyFormat { get; }
    [JsonPropertyName("currencySymbol")]
    public string CurrencySymbol { get; }
    [JsonPropertyName("currencyLayout")]
    public string CurrencyLayout { get; }
    [JsonPropertyName("thousandSeparator")]
    public string ThousandSeparator { get; }
    [JsonPropertyName("culture")]
    public string Culture { get; }
    [JsonPropertyName("role")]
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

    [JsonConstructor]
    public CashgameBunchModel(string id, string timezone, string currencyFormat, string currencySymbol, string currencyLayout, string thousandSeparator, string culture, string role)
    {
        Id = id;
        Timezone = timezone;
        CurrencyFormat = currencyFormat;
        CurrencySymbol = currencySymbol;
        CurrencyLayout = currencyLayout;
        ThousandSeparator = thousandSeparator;
        Culture = culture;
        Role = role;
    }
}