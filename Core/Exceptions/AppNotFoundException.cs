namespace Core.Exceptions
{
    public class AppNotFoundException : NotFoundException
    {
        public AppNotFoundException()
            : base(("App not found"))
        {
        }
    }
}