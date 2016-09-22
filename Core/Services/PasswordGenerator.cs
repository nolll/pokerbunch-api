namespace Core.Services
{
    public static class PasswordGenerator
    {
        private const int PasswordLength = 8;
	    
        public static string CreatePassword(string characters)
        {
            return RandomStringGenerator.GetString(PasswordLength, characters);
        }
	}
}