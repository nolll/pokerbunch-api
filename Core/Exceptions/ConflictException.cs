namespace Core.Exceptions
{
    public class ConflictException : PokerBunchException
    {
        protected ConflictException(string message) : base(message)
        {
        }
    }
}