using System.Text.Json.Serialization;

namespace Api.Models.CommonModels;

public class MessageModel
{
    [JsonPropertyName("message")]
    public virtual string Message { get; }

    public MessageModel() : this(null)
    {
    }

    public MessageModel(string? message)
    {
        Message = message ?? "";
    }
}