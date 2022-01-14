namespace Core.Services;

public class Randomizer : IRandomizer
{
    private const string RandomChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";

    public string GetAllowedChars()
    {
        return RandomChars;
    }
}