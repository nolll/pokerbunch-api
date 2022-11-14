namespace Core.Exceptions;

public class ConflictException : PokerBunchException
{
    public ConflictException(string message) : base(message)
    {
    }
}