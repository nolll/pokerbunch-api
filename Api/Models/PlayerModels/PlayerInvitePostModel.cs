using Newtonsoft.Json;

namespace Api.Models.PlayerModels;

public class PlayerInvitePostModel
{
    public string Email { get; }

    [JsonConstructor]
    public PlayerInvitePostModel(string email)
    {
        Email = email;
    }
}