using Core.Errors;

namespace Tests.Core.UseCases.CashoutTests;

public class InvalidStack : Arrange
{
    protected override int CashoutStack => -1;

    [Test]
    public void ReturnsError()
    {
        Result?.Success.Should().BeFalse();
        Result?.Error?.Type.Should().Be(ErrorType.Validation);
    }
}