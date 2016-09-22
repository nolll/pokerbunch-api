namespace Core.Exceptions
{
    public class LoginException : PokerBunchException
    {
        public LoginException()
            : base(("There was something wrong with your username or password. Please try again."))
        {
        }
    }
}