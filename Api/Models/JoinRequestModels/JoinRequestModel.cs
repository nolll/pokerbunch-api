using System.Text.Json.Serialization;

namespace Api.Models.JoinRequestModels;

public class JoinRequestModel
{
    [JsonPropertyName("id")]
    public string Id { get; }

    [JsonPropertyName("bunchId")]
    public string BunchId { get; }

    [JsonPropertyName("userId")]
    public string UserId { get; }
    
    [JsonPropertyName("userName")]
    public string UserName { get; }
    
    public JoinRequestModel(string id, string bunchId, string userId, string userName)
    {
        Id = id;
        BunchId = bunchId;
        UserId = userId;
        UserName = userName;
    }
}