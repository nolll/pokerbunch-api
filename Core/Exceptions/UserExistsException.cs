namespace Core.Exceptions
{
    public class UserExistsException : PokerBunchException
    {
        public UserExistsException()
            : base("The User Name is in use")
        {
        }
    }
}