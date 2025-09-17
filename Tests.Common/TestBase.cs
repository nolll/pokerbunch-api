using NUnit.Framework;

namespace Tests.Common;

public class TestBase
{
    protected TestDependencies Deps { get; private set; } = new();
    protected readonly TestDataFactory Create = new();

    [SetUp]
    public void ClearFakes()
    {
        Deps = new TestDependencies();
    }
}