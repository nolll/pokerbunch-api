namespace Core.Services
{
    public static class SlugGenerator
    {
        public static string GetSlug(string displayName)
        {
            if (displayName == null)
                return null;
            return displayName.Replace(" ", "-").ToLower();
        }
    }
}