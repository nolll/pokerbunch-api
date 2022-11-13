using Newtonsoft.Json;

namespace Api.Models.LocationModels;

public class LocationAddPostModel
{
    public string Name { get; }

    [JsonConstructor]
    public LocationAddPostModel(string name)
    {
        Name = name;
    }
}