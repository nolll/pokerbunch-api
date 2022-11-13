using System.Text.Json.Serialization;

namespace Api.Models.CommonModels;

public abstract class MessageModel
{
    [JsonPropertyName("message")]
    public abstract string Message { get; }
}