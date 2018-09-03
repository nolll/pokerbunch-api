namespace Plumbing
{
    public class Bootstrapper
    {
        public UseCaseContainer UseCases { get; }

        public Bootstrapper(string connectionString, string smtpHost, bool useSendGrid, string sendGridApiKey)
        {
            var deps = new Dependencies(connectionString, smtpHost, useSendGrid, sendGridApiKey);
            UseCases = new UseCaseContainer(deps);
        }
    }
}