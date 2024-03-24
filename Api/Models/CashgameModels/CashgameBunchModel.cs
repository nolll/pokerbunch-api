using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels;

[method: JsonConstructor]
public class CashgameBunchModel(
    string id,
    string timezone,
    string currencyFormat,
    string currencySymbol,
    string currencyLayout,
    string thousandSeparator,
    string culture,
    string role)
{
    [JsonPropertyName("id")]
    public string Id { get; } = id;

    [JsonPropertyName("timezone")]
    public string Timezone { get; } = timezone;

    [JsonPropertyName("currencyFormat")]
    public string CurrencyFormat { get; } = currencyFormat;

    [JsonPropertyName("currencySymbol")]
    public string CurrencySymbol { get; } = currencySymbol;

    [JsonPropertyName("currencyLayout")]
    public string CurrencyLayout { get; } = currencyLayout;

    [JsonPropertyName("thousandSeparator")]
    public string ThousandSeparator { get; } = thousandSeparator;

    [JsonPropertyName("culture")]
    public string Culture { get; } = culture;

    [JsonPropertyName("role")]
    public string Role { get; } = role;

    public CashgameBunchModel(CashgameDetails.Result detailsResult) 
        : this(
            detailsResult.Slug,
            detailsResult.Timezone,
            detailsResult.CurrencyFormat,
            detailsResult.CurrencySymbol,
            detailsResult.CurrencyLayout,
            detailsResult.ThousandSeparator,
            detailsResult.Culture,
            detailsResult.Role.ToString()
                .ToLower())
    {
    }
}