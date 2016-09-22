namespace Core.Exceptions
{
    public class CashgameNotFoundException : NotFoundException
    {
        public CashgameNotFoundException(int bunchId, string dateStr)
            : base(GetMessage(bunchId, dateStr))
        {
        }

        private static string GetMessage(int bunchId, string dateStr)
        {
            return string.Format("Cashgame not found: bunch = {0}, date = '{1}'", bunchId, dateStr);
        }
    }
}