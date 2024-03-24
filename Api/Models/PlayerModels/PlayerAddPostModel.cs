using Newtonsoft.Json;

namespace Api.Models.PlayerModels;

[method: JsonConstructor]
public class PlayerAddPostModel(string name)
{
    public string Name { get; } = name;
}