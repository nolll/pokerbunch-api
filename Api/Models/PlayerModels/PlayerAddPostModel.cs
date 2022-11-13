using Newtonsoft.Json;

namespace Api.Models.PlayerModels;

public class PlayerAddPostModel
{
    public string Name { get; }

    [JsonConstructor]
    public PlayerAddPostModel(string name)
    {
        Name = name;
    }
}