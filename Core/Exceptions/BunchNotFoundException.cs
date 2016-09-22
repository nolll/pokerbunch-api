namespace Core.Exceptions
{
    public class BunchNotFoundException : NotFoundException
    {
        public BunchNotFoundException(string slug)
            : base(GetMessage(slug))
        {
        }

        private static string GetMessage(string slug)
        {
            return string.Format("Bunch not found: slug = '{0}'", slug);
        }
    }
}