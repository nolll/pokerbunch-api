using Core.Errors;

namespace Tests.Core.UseCases.ClearCacheTests;

public class WithPlayer : Arrange
{
    protected override bool CanClearCache => false;

    protected override void Setup()
    {
    }
    
    [Test]
    public void ReturnsError()
    {
        Assert.That(Result?.Success, Is.False);
        Assert.That(Result?.Error?.Type, Is.EqualTo(ErrorType.AccessDenied));
    }
}