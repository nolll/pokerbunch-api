using NUnit.Framework;

namespace Tests.Common
{
    public class TestBase
    {
        protected TestDependencies Deps { get; private set; }

        [SetUp]
        public void ClearFakes()
        {
            Deps = new TestDependencies();
        }
    }
}
