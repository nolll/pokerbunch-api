namespace Core.Exceptions
{
    public class BunchExistsException : PokerBunchException
    {
        public BunchExistsException()
            : base("The Bunch name is not available")
        {
        }
    }
}