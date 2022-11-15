namespace Core.Exceptions;

public class AccessDeniedException : PokerBunchException
{
    public AccessDeniedException()
    {
    }

    public AccessDeniedException(string message) : base(message)
    {
    }
}
