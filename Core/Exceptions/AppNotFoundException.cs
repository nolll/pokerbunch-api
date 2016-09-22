namespace Core.Exceptions
{
    public class AppNotFoundException : PokerBunchException
    {
        public AppNotFoundException()
            : base(("App not found"))
        {
        }
    }
}