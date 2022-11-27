using Newtonsoft.Json;

namespace Api.Models.EventModels;

public class EventAddPostModel
{
    public string Name { get; }

    [JsonConstructor]
    public EventAddPostModel(string name)
    {
        Name = name;
    }
}