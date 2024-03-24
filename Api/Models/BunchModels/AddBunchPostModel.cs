using System.Text.Json.Serialization;

namespace Api.Models.BunchModels;

[method: JsonConstructor]
public class AddBunchPostModel(
    string name,
    string description,
    string timezone,
    string currencySymbol,
    string currencyLayout)
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    public string Timezone { get; } = timezone;
    public string CurrencySymbol { get; } = currencySymbol;
    public string CurrencyLayout { get; } = currencyLayout;
}