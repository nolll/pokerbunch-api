namespace Tests.Core.UseCases.ClearCacheTests;

public class WithAdmin : Arrange
{
    protected override bool CanClearCache => true;

    [Test]
    public void NoException()
    {
        Result?.Success.Should().BeTrue();
    }
}