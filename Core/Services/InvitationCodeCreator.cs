using Core.Entities;

namespace Core.Services
{
    public static class InvitationCodeCreator
    {
        private const string Salt = "0lsns5kjdl";

        public static string GetCode(Player player)
        {
            return EncryptionService.Encrypt(player.DisplayName, Salt);
        }
    }
}