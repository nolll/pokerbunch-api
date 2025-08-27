namespace Api.Auth;

public static class AuthSecretProvider
{
    // todo: Injection?
    private static readonly string SecretInTest = Guid.NewGuid().ToString();

    public static string GetSecret(string? configSecret = null)
    {
        return !string.IsNullOrEmpty(configSecret)
            ? configSecret
            : SecretInTest;
    }
}