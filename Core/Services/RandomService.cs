namespace Core.Services
{
    public class RandomService : IRandomService
    {
        private const string RandomChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";

        public string GetAllowedChars()
        {
            return RandomChars;
        }
    }
}