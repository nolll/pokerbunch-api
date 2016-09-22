using NUnit.Framework;

namespace Tests.Common
{
    public class TestBase
    {
        protected RepositoryContainer Repos { get; private set; }
        protected ServiceContainer Services { get; private set; }

        [SetUp]
        public void ClearFakes()
        {
            Repos = new RepositoryContainer();
            Services = new ServiceContainer(Repos);
        }
    }
}
