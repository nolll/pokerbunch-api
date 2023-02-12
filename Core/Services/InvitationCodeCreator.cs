using Core.Entities;

namespace Core.Services;

public class InvitationCodeCreator : IInvitationCodeCreator
{
    private readonly ISettings _settings;

    public InvitationCodeCreator(ISettings settings)
    {
        _settings = settings;
    }

    public string GetCode(Player player)
    {
        var name = player.DisplayName ?? player.Id;
        return EncryptionService.Encrypt(name, _settings.InvitationSecret);
    }
}