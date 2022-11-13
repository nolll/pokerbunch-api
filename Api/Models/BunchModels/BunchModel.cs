using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.BunchModels;

public class BunchModel
{
    [JsonPropertyName("id")]
    public string Id { get; }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("description")]
    public string Description { get; }

    [JsonPropertyName("houseRules")]
    public string HouseRules { get; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; }

    [JsonPropertyName("currencySymbol")]
    public string CurrencySymbol { get; }

    [JsonPropertyName("currencyLayout")]
    public string CurrencyLayout { get; }

    [JsonPropertyName("currencyFormat")]
    public string CurrencyFormat { get; }

    [JsonPropertyName("thousandSeparator")]
    public string ThousandSeparator { get; }

    [JsonPropertyName("defaultBuyin")]
    public int DefaultBuyin { get; }

    [JsonPropertyName("player")]
    public BunchPlayerModel Player { get; }

    [JsonPropertyName("role")]
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

    [JsonConstructor]
    public BunchModel(string id, string name, string description, string houseRules, string timezone, string currencySymbol, string currencyLayout, string currencyFormat, string thousandSeparator, int defaultBuyin, BunchPlayerModel player, string role)
    {
        Id = id;
        Name = name;
        Description = description;
        HouseRules = houseRules;
        Timezone = timezone;
        CurrencySymbol = currencySymbol;
        CurrencyLayout = currencyLayout;
        CurrencyFormat = currencyFormat;
        ThousandSeparator = thousandSeparator;
        DefaultBuyin = defaultBuyin;
        Player = player;
        Role = role;
    }

    public class BunchPlayerModel
    {
        [JsonPropertyName("id")]
        public string Id { get; }
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonConstructor]
        public BunchPlayerModel(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}