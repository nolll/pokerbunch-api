using Newtonsoft.Json;

namespace Api.Models.EventModels;

[method: JsonConstructor]
public class EventAddPostModel(string name)
{
    public string Name { get; } = name;
}