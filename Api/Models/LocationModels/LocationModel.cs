using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.LocationModels;

public class LocationModel
{
    [JsonPropertyName("id")]
    public int Id { get; }
    
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("bunch")]
    public string Bunch { get; }

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

    [JsonConstructor]
    public LocationModel(int id, string name, string bunch)
    {
        Id = id;
        Name = name;
        Bunch = bunch;
    }
}