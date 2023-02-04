using NUnit.Framework;

namespace Tests.Common;

public class TestBase
{
    protected TestDependencies Deps { get; private set; }

    public TestBase()
    {
        Deps = new TestDependencies();
    }

    [SetUp]
    public void ClearFakes()
    {
        Deps = new TestDependencies();
    }
}