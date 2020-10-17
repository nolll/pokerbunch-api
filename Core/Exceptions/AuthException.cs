namespace Core.Exceptions
{
    public class AuthException : PokerBunchException
    {
        public AuthException(string message)
            : base(message)
        {
        }
    }
}