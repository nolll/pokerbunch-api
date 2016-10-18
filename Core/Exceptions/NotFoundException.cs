namespace Core.Exceptions
{
    public class NotFoundException : PokerBunchException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}