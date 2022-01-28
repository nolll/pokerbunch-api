namespace Core.Services;

public static class GravatarService
{
    public static string GetAvatarUrl(string email)
    {
        var hash = EncryptionService.GetMd5Hash(email);
        const int size = 100;
        const string defaultMode = "blank";

        return $"https://gravatar.com/avatar/{hash}?s={size}&d={defaultMode}";
    }
}