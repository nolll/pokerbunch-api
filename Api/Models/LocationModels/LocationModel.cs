using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.LocationModels;

[method: JsonConstructor]
public class LocationModel(string id, string name, string bunch)
{
    [JsonPropertyName("id")]
    public string Id { get; } = id;

    [JsonPropertyName("name")]
    public string Name { get; } = name;

    [JsonPropertyName("bunch")]
    public string Bunch { get; } = bunch;

    public LocationModel(GetLocationList.Location location)
        : this(location.Id, location.Name, location.Slug)
    {
    }

    public LocationModel(GetLocation.Result location)
        : this(location.Id, location.Name, location.Slug)
    {
    }

    public LocationModel(AddLocation.Result location)
        : this(location.Id, location.Name, location.Slug)
    {
    }
}