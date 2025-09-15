using Core.Errors;

namespace Tests.Core.UseCases.CurrentCashgamesTests;

public class NoAccess : Arrange
{
    [Test]
    public void ReturnsAccessDeniedError()
    {
        Result!.Success.Should().BeFalse();
        Result!.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
}