namespace Core.Exceptions
{
    public class CashgameNotRunningException : PokerBunchException
    {
        public CashgameNotRunningException()
            : base("Cashgame is not running")
        {
        }
    }
}