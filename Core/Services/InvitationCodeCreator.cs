using Core.Entities;

namespace Core.Services;

public class InvitationCodeCreator(ISettings settings) : IInvitationCodeCreator
{
    public string GetCode(Player player)
    {
        var name = player.DisplayName ?? player.Id;
        return EncryptionService.Encrypt(name, settings.InvitationSecret);
    }
}