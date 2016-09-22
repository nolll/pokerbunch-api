namespace Core.Exceptions
{
    public class UserNotFoundException : PokerBunchException
    {
        public UserNotFoundException()
            : base(("User not found"))
        {
        }
    }
}