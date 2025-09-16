using AutoFixture;
using NUnit.Framework;

namespace Tests.Common;

public class TestBase
{
    protected TestDependencies Deps { get; private set; }
    protected readonly Fixture Fixture = new();
    protected readonly TestDataFactory Create = new();

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