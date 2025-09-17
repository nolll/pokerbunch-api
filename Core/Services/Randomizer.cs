namespace Core.Services;

public class Randomizer : IRandomizer
{
    public string GetAllowedChars() => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";
}