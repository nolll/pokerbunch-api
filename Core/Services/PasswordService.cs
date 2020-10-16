namespace Core.Services
{
    public static class PasswordService
    {
        private const int PasswordLength = 8;
	    
        public static string CreatePassword(string characters)
        {
            return RandomStringGenerator.GetString(PasswordLength, characters);
        }

        public static bool IsValid(string clearText, string salt, string encrypted)
        {
            var encryptedPassword = EncryptionService.Encrypt(clearText, salt);
            return encryptedPassword == encrypted;
        }
	}
}