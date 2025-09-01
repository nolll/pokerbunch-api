using System.Text.Json.Serialization;

namespace Api.Models.LocationModels;

[method: JsonConstructor]
public class LocationAddPostModel(string name)
{
    public string Name { get; } = name;
}