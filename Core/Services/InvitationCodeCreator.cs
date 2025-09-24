using Core.Entities;

namespace Core.Services;

public class InvitationCodeCreator(ISettings settings) : IInvitationCodeCreator
{
    public string GetCode(Player player) => 
        EncryptionService.Encrypt(player.DisplayName, settings.InvitationSecret);
}