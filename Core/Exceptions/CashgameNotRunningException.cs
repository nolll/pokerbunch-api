namespace Core.Exceptions
{
    public class CashgameNotRunningException : ConflictException
    {
        public CashgameNotRunningException()
            : base("Cashgame is not running")
        {
        }
    }
}