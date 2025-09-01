using System.Text.Json.Serialization;

namespace Api.Models.PlayerModels;

[method: JsonConstructor]
public class PlayerAddPostModel(string name)
{
    public string Name { get; } = name;
}