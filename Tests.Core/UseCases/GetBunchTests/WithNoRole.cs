using Core.Errors;

namespace Tests.Core.UseCases.GetBunchTests;

public class WithNoRole : Arrange
{
    [Test]
    public void AccessDenied() => Result?.Error?.Type.Should().Be(ErrorType.AccessDenied);
}