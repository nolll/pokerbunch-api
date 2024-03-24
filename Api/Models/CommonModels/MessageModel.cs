using System.Text.Json.Serialization;

namespace Api.Models.CommonModels;

public class MessageModel(string? message)
{
    [JsonPropertyName("message")]
    public virtual string Message { get; } = message ?? "";

    public MessageModel() : this(null)
    {
    }
}