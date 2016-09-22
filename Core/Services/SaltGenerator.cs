namespace Core.Services
{
	public static class SaltGenerator
	{
	    private const int SaltLength = 10;

        public static string CreateSalt(string saltCharacters)
        {
            return RandomStringGenerator.GetString(SaltLength, saltCharacters);
        }
	}
}