using JetBrains.Annotations;

namespace Api.Models.PlayerModels;

public class PlayerInvitePostModel
{
    public string Email { get; [UsedImplicitly] set; }
}