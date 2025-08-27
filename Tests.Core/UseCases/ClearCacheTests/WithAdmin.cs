namespace Tests.Core.UseCases.ClearCacheTests;

public class WithAdmin : Arrange
{
    protected override bool IsAdmin => true;

    protected override void Setup()
    {
    }
    
    [Test]
    public void NoException()
    {
        Execute();
    }
}