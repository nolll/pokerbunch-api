namespace Api.Bootstrapping;

public static class AuthSecretProvider
{
    // todo: Injection?
    private static readonly string SecretInTest = new Guid().ToString();

    public static string GetSecret(string? configSecret = null)
    {
        return !string.IsNullOrEmpty(configSecret)
            ? configSecret
            : SecretInTest;
    }
}