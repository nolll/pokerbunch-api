using Core.Errors;

namespace Tests.Core.UseCases.TestEmailTests;

public class WithPlayer : Arrange
{
    protected override bool IsAdmin => false;

    [Test]
    public void ReturnsError()
    {
        Assert.That(Result?.Error?.Type, Is.EqualTo(ErrorType.AccessDenied));
    }
}