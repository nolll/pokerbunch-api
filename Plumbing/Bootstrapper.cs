namespace Plumbing
{
    public class Bootstrapper
    {
        public UseCaseContainer UseCases { get; private set; }

        public Bootstrapper(string connectionString, string smtpHost, string smtpUserName, string smtpPassword)
        {
            var deps = new Dependencies(connectionString, smtpHost, smtpUserName, smtpPassword);
            UseCases = new UseCaseContainer(deps);
        }
    }
}