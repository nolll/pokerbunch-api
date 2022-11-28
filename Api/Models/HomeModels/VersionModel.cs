using System.Text.Json.Serialization;

namespace Api.Models.HomeModels;

public class VersionModel
{
    [JsonPropertyName("version")]
    public string Version { get; }
        
    public VersionModel(string version)
    {
        Version = "";
    }
}