namespace Core.Services;

public static class SlugGenerator
{
    public static string GetSlug(string displayName)
    {
        return displayName?.Replace(" ", "-").ToLower();
    }
}