using System.Text.Json.Serialization;

namespace Api.Models.BunchModels;

[method: JsonConstructor]
public class UpdateBunchPostModel(
    string description,
    string houseRules,
    string timezone,
    string currencySymbol,
    string currencyLayout,
    int defaultBuyin)
{
    public string Description { get; } = description;
    public string HouseRules { get; } = houseRules;
    public string Timezone { get; } = timezone;
    public string CurrencySymbol { get; } = currencySymbol;
    public string CurrencyLayout { get; } = currencyLayout;
    public int DefaultBuyin { get; } = defaultBuyin;
}