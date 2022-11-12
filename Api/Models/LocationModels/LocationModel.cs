using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.LocationModels;

public class LocationModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("bunch")]
    public string Bunch { get; set; }

    public LocationModel()
    {
    }

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

    private LocationModel(int id, string name, string bunch)
    {
        Id = id;
        Name = name;
        Bunch = bunch;
    }
}