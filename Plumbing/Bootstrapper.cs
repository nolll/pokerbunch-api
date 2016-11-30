namespace Plumbing
{
    public class Bootstrapper
    {
        public UseCaseContainer UseCases { get; private set; }

        public Bootstrapper(string connectionString)
        {
            var deps = new Dependencies(connectionString);
            UseCases = new UseCaseContainer(deps);
        }
    }
}