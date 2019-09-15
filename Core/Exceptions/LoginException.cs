namespace Core.Exceptions
{
    public class LoginException : AccessDeniedException
    {
        public LoginException()
            : base("There was something wrong with your username or password. Please try again.")
        {
        }
    }
}