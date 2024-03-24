using Newtonsoft.Json;

namespace Api.Models.PlayerModels;

[method: JsonConstructor]
public class PlayerInvitePostModel(string email)
{
    public string Email { get; } = email;
}