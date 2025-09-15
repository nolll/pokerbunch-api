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
        Result!.Success.Should().BeFalse();
        Result!.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
}