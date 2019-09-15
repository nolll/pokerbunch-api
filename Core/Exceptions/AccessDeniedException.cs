namespace Core.Exceptions
{
    public class AccessDeniedException : PokerBunchException
    {
        public AccessDeniedException()
        {
        }

        protected AccessDeniedException(string message) : base(message)
        {
        }
    }
}