using Core.Errors;

namespace Tests.Core.UseCases.ReportTests;

public class WithInvalidStack : Arrange
{
    protected override int Stack => -1;

    [Test]
    public void ReturnsValidationError() => Result?.Error?.Type.Should().Be(ErrorType.Validation);
}