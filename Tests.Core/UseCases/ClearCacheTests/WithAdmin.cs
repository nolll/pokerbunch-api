using Core.Entities;

namespace Tests.Core.UseCases.ClearCacheTests;

public class WithAdmin : Arrange
{
    protected override Role Role => Role.Admin;

    [Test]
    public void NoException()
    {
        Execute();
    }
}