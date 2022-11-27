using Core.Errors;

namespace Tests.Core.UseCases.CurrentCashgamesTests;

public class WithGuest : Arrange
{
    [Test]
    public void ReturnsAccessDeniedError()
    {
        Assert.That(Result.Success, Is.False);
        Assert.That(Result.Error.Type, Is.EqualTo(ErrorType.AccessDenied));
    }
}