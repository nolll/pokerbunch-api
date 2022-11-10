using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.BunchModels;

public class BunchModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("houseRules")]
    public string HouseRules { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }

    [JsonPropertyName("currencySymbol")]
    public string CurrencySymbol { get; set; }

    [JsonPropertyName("currencyLayout")]
    public string CurrencyLayout { get; set; }

    [JsonPropertyName("currencyFormat")]
    public string CurrencyFormat { get; set; }

    [JsonPropertyName("thousandSeparator")]
    public string ThousandSeparator { get; set; }

    [JsonPropertyName("defaultBuyin")]
    public int DefaultBuyin { get; set; }

    [JsonPropertyName("player")]
    public BunchPlayerModel Player { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }

    public BunchModel()
    {
    }

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
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public BunchPlayerModel()
        {
        }

        public BunchPlayerModel(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}