using System.Text.Json.Serialization;

namespace Api.Models.BunchModels;

public class AddBunchPostModel
{
    public string Name { get; }
    public string Description { get; }
    public string Timezone { get; }
    public string CurrencySymbol { get; }
    public string CurrencyLayout { get; }

    [JsonConstructor]
    public AddBunchPostModel(string name, string description, string timezone, string currencySymbol, string currencyLayout)
    {
        Name = name;
        Description = description;
        Timezone = timezone;
        CurrencySymbol = currencySymbol;
        CurrencyLayout = currencyLayout;
    }
}