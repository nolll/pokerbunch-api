using System.Text.Json.Serialization;

namespace Api.Models.BunchModels;

public class UpdateBunchPostModel
{
    public string Description { get; }
    public string HouseRules { get; }
    public string Timezone { get; }
    public string CurrencySymbol { get; }
    public string CurrencyLayout { get; }
    public int DefaultBuyin { get; }

    [JsonConstructor]
    public UpdateBunchPostModel(string description, string houseRules, string timezone, string currencySymbol, string currencyLayout, int defaultBuyin)
    {
        Description = description;
        HouseRules = houseRules;
        Timezone = timezone;
        CurrencySymbol = currencySymbol;
        CurrencyLayout = currencyLayout;
        DefaultBuyin = defaultBuyin;
    }
}