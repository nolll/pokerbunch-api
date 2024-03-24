using Newtonsoft.Json;

namespace Api.Models.LocationModels;

[method: JsonConstructor]
public class LocationAddPostModel(string name)
{
    public string Name { get; } = name;
}