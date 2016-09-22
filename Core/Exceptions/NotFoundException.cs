namespace Core.Exceptions
{
    public abstract class NotFoundException : PokerBunchException
    {
        protected NotFoundException(string message) : base(message)
        {
        }
    }
}