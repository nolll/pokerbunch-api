using Core.Errors;

namespace Tests.Core.UseCases.TestEmailTests;

public class WithPlayer : Arrange
{
    protected override bool CanSendTestEmail => false;

    [Test]
    public void ReturnsError() => Result?.Error?.Type.Should().Be(ErrorType.AccessDenied);
}